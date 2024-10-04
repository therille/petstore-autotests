using System;
using PetStoreAutotests.Utilities;
using Xunit.Abstractions;

namespace PetStoreAutotests.Tests
{
    public abstract class BaseTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        protected readonly string timeDateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff+00:00";
        protected IDisposable logCapture;

        protected BaseTest(ITestOutputHelper outputHelper)
        {
            _output = outputHelper;
            logCapture = new OutputHelper().Capture(_output);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                logCapture.Dispose();
            }
        }
    }
}