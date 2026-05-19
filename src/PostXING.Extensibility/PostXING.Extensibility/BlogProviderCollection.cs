using System;
using System.Collections;

namespace PostXING.Extensibility;

[Serializable]
public class BlogProviderCollection : IIBlogProviderList, IIBlogProviderCollection, IList, ICollection, IEnumerable, ICloneable
{
	private enum Tag
	{
		Default
	}

	[Serializable]
	private sealed class Enumerator : IIBlogProviderEnumerator, IEnumerator
	{
		private readonly BlogProviderCollection _collection;

		private readonly int _version;

		private int _index;

		public IBlogProvider Current
		{
			get
			{
				_collection.CheckEnumIndex(_index);
				_collection.CheckEnumVersion(_version);
				return _collection[_index];
			}
		}

		object IEnumerator.Current => Current;

		internal Enumerator(BlogProviderCollection collection)
		{
			_collection = collection;
			_version = collection._version;
			_index = -1;
		}

		public bool MoveNext()
		{
			_collection.CheckEnumVersion(_version);
			return ++_index < _collection.Count;
		}

		public void Reset()
		{
			_collection.CheckEnumVersion(_version);
			_index = -1;
		}
	}

	[Serializable]
	private sealed class ReadOnlyList : BlogProviderCollection
	{
		private BlogProviderCollection _collection;

		protected override IBlogProvider[] InnerArray => _collection.InnerArray;

		public override int Capacity
		{
			get
			{
				return _collection.Capacity;
			}
			set
			{
				throw new NotSupportedException("Read-only collections cannot be modified.");
			}
		}

		public override int Count => _collection.Count;

		public override bool IsFixedSize => true;

		public override bool IsReadOnly => true;

		public override bool IsSynchronized => _collection.IsSynchronized;

		public override bool IsUnique => _collection.IsUnique;

		public override IBlogProvider this[int index]
		{
			get
			{
				return _collection[index];
			}
			set
			{
				throw new NotSupportedException("Read-only collections cannot be modified.");
			}
		}

		public override object SyncRoot => _collection.SyncRoot;

		internal ReadOnlyList(BlogProviderCollection collection)
			: base(Tag.Default)
		{
			_collection = collection;
		}

		public override int Add(IBlogProvider value)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void AddRange(BlogProviderCollection collection)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void AddRange(IBlogProvider[] array)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override int BinarySearch(IBlogProvider value)
		{
			return _collection.BinarySearch(value);
		}

		public override void Clear()
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override object Clone()
		{
			return new ReadOnlyList((BlogProviderCollection)_collection.Clone());
		}

		public override void CopyTo(IBlogProvider[] array)
		{
			_collection.CopyTo(array);
		}

		public override void CopyTo(IBlogProvider[] array, int arrayIndex)
		{
			_collection.CopyTo(array, arrayIndex);
		}

		public override IBlogProvider GetByKey(string key)
		{
			return _collection.GetByKey(key);
		}

		public override IIBlogProviderEnumerator GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		public override int IndexOf(IBlogProvider value)
		{
			return _collection.IndexOf(value);
		}

		public override int IndexOfKey(string key)
		{
			return _collection.IndexOfKey(key);
		}

		public override void Insert(int index, IBlogProvider value)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Remove(IBlogProvider value)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void RemoveAt(int index)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void RemoveRange(int index, int count)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Reverse()
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Reverse(int index, int count)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Sort()
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Sort(IComparer comparer)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}

		public override IBlogProvider[] ToArray()
		{
			return _collection.ToArray();
		}

		public override void TrimToSize()
		{
			throw new NotSupportedException("Read-only collections cannot be modified.");
		}
	}

	[Serializable]
	private sealed class SyncList : BlogProviderCollection
	{
		private BlogProviderCollection _collection;

		private object _root;

		protected override IBlogProvider[] InnerArray
		{
			get
			{
				lock (_root)
				{
					return _collection.InnerArray;
				}
			}
		}

		public override int Capacity
		{
			get
			{
				lock (_root)
				{
					return _collection.Capacity;
				}
			}
			set
			{
				lock (_root)
				{
					_collection.Capacity = value;
				}
			}
		}

		public override int Count
		{
			get
			{
				lock (_root)
				{
					return _collection.Count;
				}
			}
		}

		public override bool IsFixedSize => _collection.IsFixedSize;

		public override bool IsReadOnly => _collection.IsReadOnly;

		public override bool IsSynchronized => true;

		public override bool IsUnique => _collection.IsUnique;

		public override IBlogProvider this[int index]
		{
			get
			{
				lock (_root)
				{
					return _collection[index];
				}
			}
			set
			{
				lock (_root)
				{
					_collection[index] = value;
				}
			}
		}

		public override object SyncRoot => _root;

		internal SyncList(BlogProviderCollection collection)
			: base(Tag.Default)
		{
			_root = collection.SyncRoot;
			_collection = collection;
		}

		public override int Add(IBlogProvider value)
		{
			lock (_root)
			{
				return _collection.Add(value);
			}
		}

		public override void AddRange(BlogProviderCollection collection)
		{
			lock (_root)
			{
				_collection.AddRange(collection);
			}
		}

		public override void AddRange(IBlogProvider[] array)
		{
			lock (_root)
			{
				_collection.AddRange(array);
			}
		}

		public override int BinarySearch(IBlogProvider value)
		{
			lock (_root)
			{
				return _collection.BinarySearch(value);
			}
		}

		public override void Clear()
		{
			lock (_root)
			{
				_collection.Clear();
			}
		}

		public override object Clone()
		{
			lock (_root)
			{
				return new SyncList((BlogProviderCollection)_collection.Clone());
			}
		}

		public override void CopyTo(IBlogProvider[] array)
		{
			lock (_root)
			{
				_collection.CopyTo(array);
			}
		}

		public override void CopyTo(IBlogProvider[] array, int arrayIndex)
		{
			lock (_root)
			{
				_collection.CopyTo(array, arrayIndex);
			}
		}

		public override IBlogProvider GetByKey(string key)
		{
			lock (_root)
			{
				return _collection.GetByKey(key);
			}
		}

		public override IIBlogProviderEnumerator GetEnumerator()
		{
			lock (_root)
			{
				return _collection.GetEnumerator();
			}
		}

		public override int IndexOf(IBlogProvider value)
		{
			lock (_root)
			{
				return _collection.IndexOf(value);
			}
		}

		public override int IndexOfKey(string key)
		{
			lock (_root)
			{
				return _collection.IndexOfKey(key);
			}
		}

		public override void Insert(int index, IBlogProvider value)
		{
			lock (_root)
			{
				_collection.Insert(index, value);
			}
		}

		public override void Remove(IBlogProvider value)
		{
			lock (_root)
			{
				_collection.Remove(value);
			}
		}

		public override void RemoveAt(int index)
		{
			lock (_root)
			{
				_collection.RemoveAt(index);
			}
		}

		public override void RemoveRange(int index, int count)
		{
			lock (_root)
			{
				_collection.RemoveRange(index, count);
			}
		}

		public override void Reverse()
		{
			lock (_root)
			{
				_collection.Reverse();
			}
		}

		public override void Reverse(int index, int count)
		{
			lock (_root)
			{
				_collection.Reverse(index, count);
			}
		}

		public override void Sort()
		{
			lock (_root)
			{
				_collection.Sort();
			}
		}

		public override void Sort(IComparer comparer)
		{
			lock (_root)
			{
				_collection.Sort(comparer);
			}
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			lock (_root)
			{
				_collection.Sort(index, count, comparer);
			}
		}

		public override IBlogProvider[] ToArray()
		{
			lock (_root)
			{
				return _collection.ToArray();
			}
		}

		public override void TrimToSize()
		{
			lock (_root)
			{
				_collection.TrimToSize();
			}
		}
	}

	[Serializable]
	private sealed class UniqueList : BlogProviderCollection
	{
		private BlogProviderCollection _collection;

		protected override IBlogProvider[] InnerArray => _collection.InnerArray;

		public override int Capacity
		{
			get
			{
				return _collection.Capacity;
			}
			set
			{
				_collection.Capacity = value;
			}
		}

		public override int Count => _collection.Count;

		public override bool IsFixedSize => _collection.IsFixedSize;

		public override bool IsReadOnly => _collection.IsReadOnly;

		public override bool IsSynchronized => _collection.IsSynchronized;

		public override bool IsUnique => true;

		public override IBlogProvider this[int index]
		{
			get
			{
				return _collection[index];
			}
			set
			{
				CheckUnique(index, value);
				_collection[index] = value;
			}
		}

		public override object SyncRoot => _collection.SyncRoot;

		internal UniqueList(BlogProviderCollection collection)
			: base(Tag.Default)
		{
			_collection = collection;
		}

		public override int Add(IBlogProvider value)
		{
			CheckUnique(value);
			return _collection.Add(value);
		}

		public override void AddRange(BlogProviderCollection collection)
		{
			IIBlogProviderEnumerator enumerator = collection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IBlogProvider current = enumerator.Current;
					CheckUnique(current);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			_collection.AddRange(collection);
		}

		public override void AddRange(IBlogProvider[] array)
		{
			foreach (IBlogProvider value in array)
			{
				CheckUnique(value);
			}
			_collection.AddRange(array);
		}

		public override int BinarySearch(IBlogProvider value)
		{
			return _collection.BinarySearch(value);
		}

		public override void Clear()
		{
			_collection.Clear();
		}

		public override object Clone()
		{
			return new UniqueList((BlogProviderCollection)_collection.Clone());
		}

		public override void CopyTo(IBlogProvider[] array)
		{
			_collection.CopyTo(array);
		}

		public override void CopyTo(IBlogProvider[] array, int arrayIndex)
		{
			_collection.CopyTo(array, arrayIndex);
		}

		public override IBlogProvider GetByKey(string key)
		{
			return _collection.GetByKey(key);
		}

		public override IIBlogProviderEnumerator GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		public override int IndexOf(IBlogProvider value)
		{
			return _collection.IndexOf(value);
		}

		public override int IndexOfKey(string key)
		{
			return _collection.IndexOfKey(key);
		}

		public override void Insert(int index, IBlogProvider value)
		{
			CheckUnique(value);
			_collection.Insert(index, value);
		}

		public override void Remove(IBlogProvider value)
		{
			_collection.Remove(value);
		}

		public override void RemoveAt(int index)
		{
			_collection.RemoveAt(index);
		}

		public override void RemoveRange(int index, int count)
		{
			_collection.RemoveRange(index, count);
		}

		public override void Reverse()
		{
			_collection.Reverse();
		}

		public override void Reverse(int index, int count)
		{
			_collection.Reverse(index, count);
		}

		public override void Sort()
		{
			_collection.Sort();
		}

		public override void Sort(IComparer comparer)
		{
			_collection.Sort(comparer);
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			_collection.Sort(index, count, comparer);
		}

		public override IBlogProvider[] ToArray()
		{
			return _collection.ToArray();
		}

		public override void TrimToSize()
		{
			_collection.TrimToSize();
		}

		private void CheckUnique(IBlogProvider value)
		{
			if (IndexOf(value) >= 0)
			{
				throw new NotSupportedException("Unique collections cannot contain duplicate elements.");
			}
		}

		private void CheckUnique(int index, IBlogProvider value)
		{
			int num = IndexOf(value);
			if (num >= 0 && num != index)
			{
				throw new NotSupportedException("Unique collections cannot contain duplicate elements.");
			}
		}
	}

	private const int _defaultCapacity = 16;

	private IBlogProvider[] _array;

	private int _count;

	[NonSerialized]
	private int _version;

	protected virtual IBlogProvider[] InnerArray => _array;

	public virtual int Capacity
	{
		get
		{
			return _array.Length;
		}
		set
		{
			if (value != _array.Length)
			{
				if (value < _count)
				{
					throw new ArgumentOutOfRangeException("Capacity", value, "Value cannot be less than Count.");
				}
				if (value == 0)
				{
					_array = new IBlogProvider[16];
					return;
				}
				IBlogProvider[] array = new IBlogProvider[value];
				Array.Copy(_array, array, _count);
				_array = array;
			}
		}
	}

	public virtual int Count => _count;

	public virtual bool IsFixedSize => false;

	public virtual bool IsReadOnly => false;

	public virtual bool IsSynchronized => false;

	public virtual bool IsUnique => false;

	public IBlogProvider this[string key] => GetByKey(key);

	public virtual IBlogProvider this[int index]
	{
		get
		{
			ValidateIndex(index);
			return _array[index];
		}
		set
		{
			ValidateIndex(index);
			_version++;
			_array[index] = value;
		}
	}

	object IList.this[int index]
	{
		get
		{
			return this[index];
		}
		set
		{
			this[index] = (IBlogProvider)value;
		}
	}

	public virtual object SyncRoot => this;

	private BlogProviderCollection(Tag tag)
	{
	}

	public BlogProviderCollection()
	{
		_array = new IBlogProvider[16];
	}

	public BlogProviderCollection(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity", capacity, "Argument cannot be negative.");
		}
		_array = new IBlogProvider[capacity];
	}

	public BlogProviderCollection(BlogProviderCollection collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		_array = new IBlogProvider[collection.Count];
		AddRange(collection);
	}

	public BlogProviderCollection(IBlogProvider[] array)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		_array = new IBlogProvider[array.Length];
		AddRange(array);
	}

	public virtual int Add(IBlogProvider value)
	{
		if (_count == _array.Length)
		{
			EnsureCapacity(_count + 1);
		}
		_version++;
		_array[_count] = value;
		return _count++;
	}

	int IList.Add(object value)
	{
		return Add((IBlogProvider)value);
	}

	public virtual void AddRange(BlogProviderCollection collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (collection.Count != 0)
		{
			if (_count + collection.Count > _array.Length)
			{
				EnsureCapacity(_count + collection.Count);
			}
			_version++;
			Array.Copy(collection.InnerArray, 0, _array, _count, collection.Count);
			_count += collection.Count;
		}
	}

	public virtual void AddRange(IBlogProvider[] array)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (array.Length != 0)
		{
			if (_count + array.Length > _array.Length)
			{
				EnsureCapacity(_count + array.Length);
			}
			_version++;
			Array.Copy(array, 0, _array, _count, array.Length);
			_count += array.Length;
		}
	}

	public virtual int BinarySearch(IBlogProvider value)
	{
		return Array.BinarySearch(_array, 0, _count, value);
	}

	public virtual void Clear()
	{
		if (_count != 0)
		{
			_version++;
			Array.Clear(_array, 0, _count);
			_count = 0;
		}
	}

	public virtual object Clone()
	{
		BlogProviderCollection blogProviderCollection = new BlogProviderCollection(_count);
		Array.Copy(_array, 0, blogProviderCollection._array, 0, _count);
		blogProviderCollection._count = _count;
		blogProviderCollection._version = _version;
		return blogProviderCollection;
	}

	public bool Contains(IBlogProvider value)
	{
		return IndexOf(value) >= 0;
	}

	bool IList.Contains(object value)
	{
		return Contains((IBlogProvider)value);
	}

	public bool ContainsKey(string key)
	{
		return IndexOfKey(key) >= 0;
	}

	public virtual void CopyTo(IBlogProvider[] array)
	{
		CheckTargetArray(array, 0);
		Array.Copy(_array, array, _count);
	}

	public virtual void CopyTo(IBlogProvider[] array, int arrayIndex)
	{
		CheckTargetArray(array, arrayIndex);
		Array.Copy(_array, 0, array, arrayIndex, _count);
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		CheckTargetArray(array, arrayIndex);
		CopyTo((IBlogProvider[])array, arrayIndex);
	}

	public virtual IBlogProvider GetByKey(string key)
	{
		int num = IndexOfKey(key);
		if (num >= 0)
		{
			return _array[num];
		}
		return null;
	}

	public virtual IIBlogProviderEnumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return (IEnumerator)GetEnumerator();
	}

	public virtual int IndexOf(IBlogProvider value)
	{
		return Array.IndexOf(_array, value, 0, _count);
	}

	int IList.IndexOf(object value)
	{
		return IndexOf((IBlogProvider)value);
	}

	public virtual int IndexOfKey(string key)
	{
		for (int i = 0; i < _count; i++)
		{
			if (_array[i] != null && _array[i].ProviderName == key)
			{
				return i;
			}
		}
		return -1;
	}

	public virtual void Insert(int index, IBlogProvider value)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot be negative.");
		}
		if (index > _count)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot exceed Count.");
		}
		if (_count == _array.Length)
		{
			EnsureCapacity(_count + 1);
		}
		_version++;
		if (index < _count)
		{
			Array.Copy(_array, index, _array, index + 1, _count - index);
		}
		_array[index] = value;
		_count++;
	}

	void IList.Insert(int index, object value)
	{
		Insert(index, (IBlogProvider)value);
	}

	public static BlogProviderCollection ReadOnly(BlogProviderCollection collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		return new ReadOnlyList(collection);
	}

	public virtual void Remove(IBlogProvider value)
	{
		int num = IndexOf(value);
		if (num >= 0)
		{
			RemoveAt(num);
		}
	}

	void IList.Remove(object value)
	{
		Remove((IBlogProvider)value);
	}

	public virtual void RemoveAt(int index)
	{
		ValidateIndex(index);
		_version++;
		if (index < --_count)
		{
			Array.Copy(_array, index + 1, _array, index, _count - index);
		}
		_array[_count] = null;
	}

	public virtual void RemoveRange(int index, int count)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot be negative.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", count, "Argument cannot be negative.");
		}
		if (index + count > _count)
		{
			throw new ArgumentException("Arguments denote invalid range of elements.");
		}
		if (count != 0)
		{
			_version++;
			_count -= count;
			if (index < _count)
			{
				Array.Copy(_array, index + count, _array, index, _count - index);
			}
			Array.Clear(_array, _count, count);
		}
	}

	public virtual void Reverse()
	{
		if (_count > 1)
		{
			_version++;
			Array.Reverse(_array, 0, _count);
		}
	}

	public virtual void Reverse(int index, int count)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot be negative.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", count, "Argument cannot be negative.");
		}
		if (index + count > _count)
		{
			throw new ArgumentException("Arguments denote invalid range of elements.");
		}
		if (count > 1 && _count > 1)
		{
			_version++;
			Array.Reverse(_array, index, count);
		}
	}

	public virtual void Sort()
	{
		if (_count > 1)
		{
			_version++;
			Array.Sort(_array, 0, _count);
		}
	}

	public virtual void Sort(IComparer comparer)
	{
		if (_count > 1)
		{
			_version++;
			Array.Sort(_array, 0, _count, comparer);
		}
	}

	public virtual void Sort(int index, int count, IComparer comparer)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot be negative.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", count, "Argument cannot be negative.");
		}
		if (index + count > _count)
		{
			throw new ArgumentException("Arguments denote invalid range of elements.");
		}
		if (count > 1 && _count > 1)
		{
			_version++;
			Array.Sort(_array, index, count, comparer);
		}
	}

	public static BlogProviderCollection Synchronized(BlogProviderCollection collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		return new SyncList(collection);
	}

	public virtual IBlogProvider[] ToArray()
	{
		IBlogProvider[] array = new IBlogProvider[_count];
		Array.Copy(_array, array, _count);
		return array;
	}

	public virtual void TrimToSize()
	{
		Capacity = _count;
	}

	public static BlogProviderCollection Unique(BlogProviderCollection collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		for (int num = collection.Count - 1; num > 0; num--)
		{
			if (collection.IndexOf(collection[num]) < num)
			{
				throw new ArgumentException("collection", "Argument cannot contain duplicate elements.");
			}
		}
		return new UniqueList(collection);
	}

	private void CheckEnumIndex(int index)
	{
		if (index < 0 || index >= _count)
		{
			throw new InvalidOperationException("Enumerator is not on a collection element.");
		}
	}

	private void CheckEnumVersion(int version)
	{
		if (version != _version)
		{
			throw new InvalidOperationException("Enumerator invalidated by modification to collection.");
		}
	}

	private void CheckTargetArray(Array array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (array.Rank > 1)
		{
			throw new ArgumentException("Argument cannot be multidimensional.", "array");
		}
		if (arrayIndex < 0)
		{
			throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "Argument cannot be negative.");
		}
		if (arrayIndex >= array.Length)
		{
			throw new ArgumentException("Argument must be less than array length.", "arrayIndex");
		}
		if (_count > array.Length - arrayIndex)
		{
			throw new ArgumentException("Argument section must be large enough for collection.", "array");
		}
	}

	private void EnsureCapacity(int minimum)
	{
		int num = ((_array.Length == 0) ? 16 : (_array.Length * 2));
		if (num < minimum)
		{
			num = minimum;
		}
		Capacity = num;
	}

	private void ValidateIndex(int index)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument cannot be negative.");
		}
		if (index >= _count)
		{
			throw new ArgumentOutOfRangeException("index", index, "Argument must be less than Count.");
		}
	}
}
