﻿using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NunitRetrying.Tests
{
    public static class TestFixture<TFixture>
        where TFixture : new()
    {
        public static RunnableTest GetTest(Expression<Action<RetryingTestMethods>> testSelector)
        {
            var methodName = ((MethodCallExpression)testSelector.Body).Method.Name;

            return GetTest(methodName);
        }

        public static RunnableTest GetTest(string testName)
        {
            var fixture = new TFixture();

            var selectedTest = FindTest(testName, fixture);

            return new RunnableTest(selectedTest, fixture);
        }

        private static ITest FindTest(string testName, TFixture fixture)
        {
            var testSuite = GetTestSuite(fixture);

            return testSuite.Tests.Single(test => test.Name == testName);
        }

        private static TestSuite GetTestSuite(TFixture fixture)
        {
            var suiteBuilder = new DefaultSuiteBuilder();

            var testSuite = suiteBuilder.BuildFrom(new TypeWrapper(typeof(TFixture)));

            testSuite.Fixture = fixture;

            return testSuite;
        }
    }
}