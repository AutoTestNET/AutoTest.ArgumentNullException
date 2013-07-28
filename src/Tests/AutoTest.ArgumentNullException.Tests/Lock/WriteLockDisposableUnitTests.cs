namespace AutoTest.ArgNullEx.Lock
{
    using System;
    using System.Threading;
    using global::Xunit;

    public class WriteLockDisposableUnitTests
    {
        [Fact]
        public void WriteLockDisposable_Constructor_DoesNotAquireLock_IfAquireLockFalse()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (new ReadLockDisposable(rwLock))
            // Act
            using (new WriteLockDisposable(rwLock, false))
            {
                // Assert
                Assert.False(rwLock.IsWriteLockHeld);
            }
        }

        [Fact]
        public void WriteLockDisposable_Lock_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (var sut = new WriteLockDisposable(rwLock, false))
            {
                // Act
                sut.Lock();

                // If an attempt to acquire the lock is made when one is already
                // acquired an exception will be thrown.
                sut.Lock();

                // Assert
                Assert.True(rwLock.IsWriteLockHeld);
            }
        }

        [Fact]
        public void WriteLockDisposable_Unlock_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            using (var sut = new WriteLockDisposable(rwLock))
            {
                // Act
                sut.Unlock();

                // If an attempt to release the lock is made when one is not
                // acquired an exception will be thrown.
                sut.Unlock();

                // Assert
                Assert.False(rwLock.IsWriteLockHeld);
            }
        }

        [Fact]
        public void WriteLockDisposable_Dispose_IsIdempotent()
        {
            // Arrange
            using (var rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion))
            {
                // Act
                var sut = new WriteLockDisposable(rwLock);

                sut.Dispose();

                // If an attempt to release the lock is made when one is not
                // acquired an exception will be thrown.
                sut.Dispose();

                // Assert
                Assert.False(rwLock.IsWriteLockHeld);
            }
        }
    }
}
