using System;
using System.Collections;
using System.Collections.Generic;

namespace NConsole.Options
{
    using static ErrorLevelDescriptor;

    public class ErrorLevelCollection : ICollection<ErrorLevelDescriptor>
    {
        private IList<ErrorLevelDescriptor> Collection { get; }

        public ErrorLevelCollection()
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            Collection = new List<ErrorLevelDescriptor> { };
        }

        private void CollectionAction(Action<ICollection<ErrorLevelDescriptor>> action) => action.Invoke(Collection);

        private TResult CollectionFunc<TResult>(Func<ICollection<ErrorLevelDescriptor>, TResult> func) => func.Invoke(Collection);

        public IEnumerator<ErrorLevelDescriptor> GetEnumerator() => CollectionFunc(x => x.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<ErrorLevelDescriptor>.Add(ErrorLevelDescriptor item) => Add(item);

        public new ErrorLevelCollection Add(ErrorLevelDescriptor item)
        {
            CollectionAction(x => x.Add(item));
            return CollectionFunc(x => this);
        }

        public new ErrorLevelCollection Add(int errorLevel, ErrorLevelFilterDelegate filterCallback
            , ErrorLevelDescriptionDelegate descriptionCallback)
            => Add(new ErrorLevelDescriptor(errorLevel, filterCallback, descriptionCallback));

        public new ErrorLevelCollection Add(int errorLevel)
            => Add(new ErrorLevelDescriptor(errorLevel, DefaultFilterCallback, DefaultDescriptionCallback));

        public void Clear() => CollectionAction(x => x.Clear());

        public bool Contains(ErrorLevelDescriptor item) => CollectionFunc(x => x.Contains(item));

        public void CopyTo(ErrorLevelDescriptor[] array, int arrayIndex) => CollectionAction(x => x.CopyTo(array, arrayIndex));

        public bool Remove(ErrorLevelDescriptor item) => CollectionFunc(x => x.Remove(item));

        public int Count => CollectionFunc(x => x.Count);

        public bool IsReadOnly => false;
    }
}
