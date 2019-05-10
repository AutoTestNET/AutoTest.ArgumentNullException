// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Lock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Provides disposable releasing of a write lock on a <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    public struct WriteLockDisposable : IDisposable
    {
        /// <summary>
        /// The <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        private readonly ReaderWriterLockSlim _slimLock;

        /// <summary>
        /// <see langword="true"/> if the write lock is acquired, otherwise <see langword="false"/>.
        /// </summary>
        private bool _lockAcquired;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLockDisposable" /> struct with the
        /// <paramref name="slimLock"/> and optionally acquires the write lock.
        /// </summary>
        /// <param name="slimLock">The <see cref="ReaderWriterLockSlim"/>.</param>
        /// <param name="aquireLock">Acquires the write lock if <see langword="true"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="slimLock"/> parameter is
        /// <see langword="null"/>.</exception>
        public WriteLockDisposable(ReaderWriterLockSlim slimLock, bool aquireLock = true)
        {
            if (slimLock == null)
                throw new ArgumentNullException(nameof(slimLock));

            _slimLock = slimLock;
            _lockAcquired = false;

            if (aquireLock)
                Lock();
        }

        /// <summary>
        /// Releases the write lock.
        /// </summary>
        public void Dispose()
        {
            Unlock();
        }

        /// <summary>
        /// Acquires a write lock on the <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        public void Lock()
        {
            if (_lockAcquired)
                return;

            _slimLock.EnterWriteLock();
            _lockAcquired = true;
        }

        /// <summary>
        /// Acquires a write lock on the <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        public void Unlock()
        {
            if (!_lockAcquired)
                return;

            _slimLock.ExitWriteLock();
            _lockAcquired = false;
        }
    }
}
