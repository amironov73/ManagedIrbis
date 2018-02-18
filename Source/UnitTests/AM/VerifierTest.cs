using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

using JetBrains.Annotations;

namespace UnitTests.AM
{
    [TestClass]
    public class VerifierTest
    {
        class Dummy
            : IVerifiable
        {
            public bool Verify(bool throwOnError)
            {
                Verifier<Dummy> verifier = new Verifier<Dummy>(this, false);

                return verifier.Result;
            }
        }

        [NotNull]
        private Verifier<Dummy> _GetVerifierNoThrow()
        {
            Dummy dummy = new Dummy();
            Verifier<Dummy> result = new Verifier<Dummy>(dummy, false);

            return result;
        }

        [NotNull]
        private Verifier<Dummy> _GetVerifierThrow()
        {
            Dummy dummy = new Dummy();
            Verifier<Dummy> result = new Verifier<Dummy>(dummy, true);

            return result;
        }

        [TestMethod]
        public void Verifier_Construction_1()
        {
            Dummy dummy = new Dummy();
            Verifier<Dummy> verifier = new Verifier<Dummy>(dummy, false);
            Assert.IsFalse(verifier.ThrowOnError);
            Assert.AreSame(dummy, verifier.Target);
            Assert.IsTrue(verifier.Result);
            Assert.IsNull(verifier.Prefix);
        }

        [TestMethod]
        public void Verifier_Prefix_1()
        {
            const string prefix = "Prefix";

            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            Assert.IsNull(verifier.Prefix);
            verifier.Prefix = prefix;
            Assert.AreEqual(prefix, verifier.Prefix);
        }

        [TestMethod]
        public void Verifier_Assert_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Assert(true);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_2()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Assert(false);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Prefix = "Prefix";
            verifier.Assert(false);
        }

        [TestMethod]
        public void Verifier_Assert_4()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Assert(true, "message");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_5()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Assert(false, "message");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_6()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Prefix = "Prefix";
            verifier.Assert(false, "message");
        }

        [TestMethod]
        public void Verifier_Assert_7()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Assert(true, "message {0}", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_8()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Assert(false, "message {0}", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Assert_9()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Prefix = "Prefix";
            verifier.Assert(false, "message {0}", 1);
        }

        [TestMethod]
        public void Verifier_DirectoryExist_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.DirectoryExist("NoSuchDirectory", "Name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_DirectoryExist_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.DirectoryExist(null, "Name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_DirectoryExist_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.DirectoryExist("NoSuchDirectory", "Name");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_DirectoryExist_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.DirectoryExist(null, "Name");
        }

        [TestMethod]
        public void Verifier_FileExist_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.FileExist("NoSuchFile", "Name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_FileExist_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.FileExist(null, "Name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_FileExist_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.FileExist("NoSuchFile", "Name");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_FileExist_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.FileExist(null, "Name");
        }

        [TestMethod]
        public void Verifier_NotNull_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNull(this);
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNull_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNull(null);
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNull_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNull(this);
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_NotNull_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNull(null, "name");
        }

        [TestMethod]
        public void Verifier_NotNull_5()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNull(this, "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNull_6()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNull(null, "name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNull_7()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNull(this);
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_NotNull_8()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNull(null, "name");
        }

        [TestMethod]
        public void Verifier_NotNullNorEmpty_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNullNorEmpty("text");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNullNorEmpty_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.NotNullNorEmpty(null);
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_NotNullNorEmpty_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNullNorEmpty("text", "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_NotNullNorEmpty_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.NotNullNorEmpty(null, "name");
        }

        [TestMethod]
        public void Verifier_Positive_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Positive(1, "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_Positive_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Positive(-1, "name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_Positive_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Positive(1, "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Positive_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Positive(-1, "name");
        }

        [TestMethod]
        public void Verifier_ReferenceEquals_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.ReferenceEquals(this, this, "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_ReferenceEquals_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.ReferenceEquals(this, new object(), "name");
            Assert.IsFalse(verifier.Result);
        }

        [TestMethod]
        public void Verifier_ReferenceEquals_3()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.ReferenceEquals(this, this, "name");
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_ReferenceEquals_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.ReferenceEquals(this, new object(), "name");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Throw();
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_2()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Throw();
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_3()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Throw("message");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_4()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Throw("message");
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_5()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            verifier.Throw("message {0}", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void Verifier_Throw_6()
        {
            Verifier<Dummy> verifier = _GetVerifierThrow();
            verifier.Throw("message {0}", 2);
        }

        [TestMethod]
        public void Verifier_VerifySubObject_1()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            Dummy subObject = new Dummy();
            verifier.VerifySubObject(subObject);
            Assert.IsTrue(verifier.Result);
        }

        [TestMethod]
        public void Verifier_VerifySubObject_2()
        {
            Verifier<Dummy> verifier = _GetVerifierNoThrow();
            Dummy subObject = new Dummy();
            verifier.VerifySubObject(subObject, "name");
            Assert.IsTrue(verifier.Result);
        }
    }
}
