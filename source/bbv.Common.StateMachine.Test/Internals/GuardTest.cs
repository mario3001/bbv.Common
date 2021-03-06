//-------------------------------------------------------------------------------
// <copyright file="GuardTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.StateMachine.Internals
{
    using Xunit;

    /// <summary>
    /// Tests the guard feature of the <see cref="StateMachine"/>.
    /// </summary>
    public class GuardTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardTest"/> class.
        /// </summary>
        public GuardTest()
        {
            this.testee = new StateMachine<States, Events>();
        }

        /// <summary>
        /// Only the transition with a guard returning true is executed and the event arguments are passed to the guard.
        /// </summary>
        [Fact]
        public void TransitionWithGuardReturningTrueIsExecuted()
        {
            this.testee.In(States.A)
                .On(Events.A)
                    .If(arguments => false).Goto(States.B)
                    .If(arguments => true).Goto(States.C)
                    .If(argument => false).Goto(States.D);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.A);

            Assert.Equal(States.C, this.testee.CurrentStateId);
        }

        [Fact]
        public void OtherwiseIsExecutedWhenNoOtherGuardReturnsTrue()
        {
            this.testee.In(States.A)
                .On(Events.A)
                    .If(arguments => false).Goto(States.B)
                    .Otherwise().Goto(States.C);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.A);

            Assert.Equal(States.C, this.testee.CurrentStateId);
        }

        /// <summary>
        /// The event arguments are passed to the guard.
        /// </summary>
        [Fact]
        public void EventArgumentsArePassedToTheGuard()
        {
            var originalEventArguments = new object[] { 1, 2, "test" };

            object[] eventArguments = null;

            this.testee.In(States.A)
                .On(Events.A)
                    .If(arguments =>
                        {
                            eventArguments = arguments;
                            return true;
                        })
                    .Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.A, originalEventArguments);

            Assert.Same(originalEventArguments, eventArguments);
        }

        /// <summary>
        /// When all guards deny the execution of its transition then ???
        /// </summary>
        [Fact]
        public void AllGuardsReturnFalse()
        {
            bool transitionDeclined = false;

            this.testee.TransitionDeclined += (sender, e) => { transitionDeclined = true; };

            this.testee.In(States.A)
                .On(Events.A).If(eventArguments => false).Goto(States.B)
                .On(Events.A).If(eventArguments => false).Goto(States.C);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.A);

            Assert.Equal(States.A, this.testee.CurrentStateId);
            Assert.True(transitionDeclined, "transition was not declined.");
        }

        [Fact]
        public void GuardWithoutArguments()
        {
            this.testee.In(States.A)
                .On(Events.B)
                    .If(() => false).Goto(States.C)
                    .If(() => true).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();
            this.testee.Fire(Events.B);

            Assert.Equal(States.B, this.testee.CurrentStateId);
        }

        [Fact]
        public void GuardWithASingleArgument()
        {
            this.testee.In(States.A)
                .On(Events.B)
                    .If<int>(SingleIntArgumentGuardReturningFalse).Goto(States.C)
                    .If(arguments => false).Goto(States.D)
                    .If(() => false).Goto(States.E)
                    .If<int>(SingleIntArgumentGuardReturningTrue).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();
            this.testee.Fire(Events.B, new object[] { 3 });

            Assert.Equal(States.B, this.testee.CurrentStateId);
        }

        private static bool SingleIntArgumentGuardReturningTrue(int i)
        {
            return true;
        }

        private static bool SingleIntArgumentGuardReturningFalse(int i)
        {
            return false;
        }
    }
}