# Dictating posts in PostXING

PostXING doesn't ship its own speech engine — it uses your operating system's built-in dictation, which is already fast, private (recognition runs on-device), and supports punctuation/formatting commands. The mic button in the editor is a **reminder and focus shortcut**: tap it to focus the editor, then start your OS dictation.

## Windows

1. Click into the editor (or tap the **mic** button in the status bar to focus it).
2. Press **`Win + H`** to open the Windows dictation toolbar.
3. Speak. Windows types the transcription straight into the editor.
4. Use voice commands for punctuation and structure — e.g. *"period"*, *"comma"*, *"new line"*, *"new paragraph"*, *"open quote" / "close quote"*.
5. Say *"stop dictation"* or press `Win + H` again to stop.

The dictation toolbar floats at the top of the screen and stays out of the editor, so you can dictate without any in-app chrome.

## Android

1. Tap into the editor (or tap the **mic** button in the top app bar to focus it).
2. On your soft keyboard, tap its **microphone** key (most Android keyboards — Gboard, etc. — put it near the space bar or top row).
3. Speak. The keyboard inserts the transcription.

PostXING's editor is written to handle voice input cleanly — it won't drop your words mid-sentence while it re-colorizes the syntax (it suppresses the highlight pass during IME composition and re-runs it once your phrase is committed).

## Notes

- **Nothing extra leaves your device.** Dictation is the OS's own recognition; PostXING only receives the text the keyboard/toolbar types, exactly as if you'd typed it.
- The mic button does **not** record or transcribe itself — it focuses the editor and reminds you of the shortcut. The actual dictation session is owned by Windows / your Android keyboard.
- If the mic key isn't on your Android keyboard, enable voice typing in your keyboard's settings (Gboard → Settings → Voice typing).
