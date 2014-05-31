namespace AutoTest.ArgNullEx.Lock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using global::Xunit;

    public class ReadLockDisposableUnitTests
    {
        [Fact]
        public void ReadLockDisposable_Constructor_DoesNotAquireLock_IfAquireLockFalse()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (new WriteLockDisposable(rwLock))
            // Act
            using (new ReadLockDisposable(rwLock, false))
            {
                // Assert
                Assert.False(rwLock.IsReadLockHeld);
            }
        }

        [Fact]
        public void ReadLockDisposable_Lock_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (var sut = new ReadLockDisposable(rwLock, false))
            {
                // Act
                sut.Lock();

                // If an attempt to acquire the lock is made when one is already
                // acquired an exception will be thrown.
                sut.Lock();

                // Assert
                Assert.True(rwLock.IsReadLockHeld);
            }
        }

        [Fact]
        public void ReadLockDisposable_Unlock_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (var sut = new ReadLockDisposable(rwLock))
            {
                // Act
                sut.Unlock();

                // If an attempt to release the lock is made when one is not
                // acquired an exception will be thrown.
                sut.Unlock();

                // Assert
                Assert.False(rwLock.IsReadLockHeld);
            }
        }

        [Fact]
        public void ReadLockDisposable_Dispose_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            {
                // Act
                var sut = new ReadLockDisposable(rwLock);

                sut.Dispose();

                // If an attempt to release the lock is made when one is not
                // acquired an exception will be thrown.
                sut.Dispose();

                // Assert
                Assert.False(rwLock.IsReadLockHeld);
            }
        }
    }
}
