using System;

namespace AlmaIntegrationTools.AccountSync.Reader
{
    /// <summary>
    /// Read input data interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPatronReader<T> : IDisposable
    {
        /// <summary>
        /// Open input stream.
        /// </summary>
        public void Open();

        /// <summary>
        /// Close input stream.
        /// </summary>
        public void Close();

        /// <summary>
        /// Read next.
        /// </summary>
        /// <returns></returns>
        public T ReadNext();
    }
}
