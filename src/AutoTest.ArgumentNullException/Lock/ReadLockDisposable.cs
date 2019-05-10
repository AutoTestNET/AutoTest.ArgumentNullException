// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Lock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Provides disposable releasing of a read lock on a <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    public struct ReadLockDisposable : IDisposable
    {
        /// <summary>
        /// The <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        private readonly ReaderWriterLockSlim _slimLock;

        /// <summary>
        /// <see langword="true"/> if the read lock is acquired; otherwise <see langword="false"/>.
        /// </summary>
        private bool _lockAcquired;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLockDisposable" /> struct with the
        /// <paramref name="slimLock"/> and optionally acquires the read lock.
        /// </summary>
        /// <param name="slimLock">The <see cref="ReaderWriterLockSlim"/>.</param>
        /// <param name="aquireLock">Acquires the read lock if <see langword="true"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="slimLock"/> parameter is
        /// <see langword="null"/>.</exception>
        public ReadLockDisposable(ReaderWriterLockSlim slimLock, bool aquireLock = true)
        {
            if (slimLock == null)
                throw new ArgumentNullException(nameof(slimLock));

            _slimLock = slimLock;
            _lockAcquired = false;

            if (aquireLock)
                Lock();
        }

        /// <summary>
        /// Releases the read lock.
        /// </summary>
        public void Dispose()
        {
            Unlock();
        }

        /// <summary>
        /// Acquires a read lock on the <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        public void Lock()
        {
            if (_lockAcquired)
                return;

            _slimLock.EnterReadLock();
            _lockAcquired = true;
        }

        /// <summary>
        /// Acquires a read lock on the <see cref="ReaderWriterLockSlim"/>.
        /// </summary>
        public void Unlock()
        {
            if (!_lockAcquired)
                return;

            _slimLock.ExitReadLock();
            _lockAcquired = false;
        }
    }
}
