using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PostXING.ViewModels;

public sealed partial class GhTerminalViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OutputText))]
    private string _output =
        "Type a gh subcommand and press Run. Example: auth status\n" +
        "For login with an existing PAT, paste it below and run: auth login --with-token\n\n";

    [ObservableProperty] private string _command = "auth status";
    [ObservableProperty] private string _stdin = string.Empty;
    [ObservableProperty] private bool _isBusy;

    public string OutputText => Output;

    public event EventHandler? CloseRequested;

    [RelayCommand(CanExecute = nameof(CanRun))]
    private async Task RunAsync()
    {
        if (string.IsNullOrWhiteSpace(Command)) return;
        IsBusy = true;
        try
        {
            var args = SplitArgs(Command);
            Append($"$ gh {Command}\n");
            var psi = new ProcessStartInfo("gh")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = !string.IsNullOrEmpty(Stdin),
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            foreach (var a in args) psi.ArgumentList.Add(a);

            using var p = new Process { StartInfo = psi };
            try
            {
                if (!p.Start())
                {
                    Append("Failed to start `gh`. Is the GitHub CLI on PATH?\n\n");
                    return;
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Append($"`gh` not found ({ex.Message}). Install from https://cli.github.com\n\n");
                return;
            }

            if (!string.IsNullOrEmpty(Stdin))
            {
                await p.StandardInput.WriteAsync(Stdin);
                p.StandardInput.Close();
            }

            var stdoutTask = p.StandardOutput.ReadToEndAsync();
            var stderrTask = p.StandardError.ReadToEndAsync();
            await p.WaitForExitAsync();
            var stdout = await stdoutTask;
            var stderr = await stderrTask;
            if (!string.IsNullOrEmpty(stdout)) Append(stdout);
            if (!string.IsNullOrEmpty(stderr)) Append(stderr);
            if (!stdout.EndsWith('\n') && !stderr.EndsWith('\n')) Append("\n");
            Append($"[exit {p.ExitCode}]\n\n");

            if (!string.IsNullOrEmpty(Stdin)) Stdin = string.Empty;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void Clear() => Output = string.Empty;

    [RelayCommand]
    private void Close() => CloseRequested?.Invoke(this, EventArgs.Empty);

    private bool CanRun() => !IsBusy && !string.IsNullOrWhiteSpace(Command);

    private void Append(string text) => Output += text;

    private static List<string> SplitArgs(string line)
    {
        var args = new List<string>();
        var sb = new StringBuilder();
        var inQuote = false;
        char quote = '"';
        foreach (var c in line)
        {
            if (inQuote)
            {
                if (c == quote) { inQuote = false; }
                else sb.Append(c);
            }
            else if (c == '"' || c == '\'')
            {
                inQuote = true;
                quote = c;
            }
            else if (char.IsWhiteSpace(c))
            {
                if (sb.Length > 0) { args.Add(sb.ToString()); sb.Clear(); }
            }
            else sb.Append(c);
        }
        if (sb.Length > 0) args.Add(sb.ToString());
        return args;
    }

    partial void OnCommandChanged(string value) => RunCommand.NotifyCanExecuteChanged();
    partial void OnIsBusyChanged(bool value) => RunCommand.NotifyCanExecuteChanged();
}
