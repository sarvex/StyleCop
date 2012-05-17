﻿//-----------------------------------------------------------------------
// <copyright file="Countdown.cs">
//   MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. If you cannot locate the  
//   Microsoft Public License, please send an email to dlr@microsoft.com. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------

namespace StyleCop
{
    using System;
    using System.Threading;

    internal class Countdown
    {
        #region Static Fields

        /// <summary>
        ///   The object we lock on.
        /// </summary>
        private static readonly object lockObject = new object();

        #endregion

        #region Fields

        /// <summary>
        ///   The countdown value we use.
        /// </summary>
        private int countdownValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Countdown" /> class with the specified count.
        /// </summary>
        /// <param name="initialCount">
        /// The number of signals initially required to set the <see cref="Countdown" />.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="initialCount" /> is less than 0.
        /// </exception>
        public Countdown(int initialCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }

            this.countdownValue = initialCount;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of remaining signals required to release the countdown.
        /// </summary>
        /// <returns>
        /// The number of remaining signals.
        /// </returns>
        public int CurrentCount
        {
            get
            {
                int num = this.countdownValue;
                return num >= 0 ? num : 0;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Increments the Countdown's current count by a specified value.
        /// </summary>
        /// <param name="signalCount"> The value by which to increase <see cref="Countdown.CurrentCount" /> . </param>
        public void AddCount(int signalCount)
        {
            lock (lockObject)
            {
                this.countdownValue += signalCount;
                if (this.countdownValue <= 0)
                {
                    Monitor.PulseAll(lockObject);
                }
            }
        }

        /// <summary>
        ///   Registers a signal with the <see cref="Countdown" /> , decrementing the value of <see cref="Countdown.CurrentCount" /> .
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The countdown was already zero.</exception>
        public void Signal()
        {
            if (this.countdownValue <= 0)
            {
                throw new InvalidOperationException("Countdown is already at zero.");
            }

            this.AddCount(-1);
        }

        /// <summary>
        /// Registers multiple signals with the <see cref="Countdown" /> , decrementing the value of <see
        /// cref="Countdown.CurrentCount" /> by the specified amount.
        /// </summary>
        /// <param name="signalCount"> The number of signals to register. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="signalCount" /> is less than 1.
        /// </exception>
        public void Signal(int signalCount)
        {
            if (signalCount <= 0)
            {
                throw new ArgumentOutOfRangeException("signalCount");
            }

            this.AddCount(signalCount);
        }

        /// <summary>
        ///   Blocks the current thread until the <see cref="Countdown" /> is set.
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
        public void Wait()
        {
            lock (lockObject)
            {
                while (this.countdownValue > 0)
                {
                    Monitor.Wait(lockObject);
                }
            }
        }

        #endregion
    }
}