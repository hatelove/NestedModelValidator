using Infra.ModelValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using UnitTestModelValidator.ModelForValidation;

namespace UnitTestModelValidator
{
    /// <summary>
    /// TestPropertyIsCollection 的摘要描述
    /// </summary>
    [TestClass]
    public class TestPropertyIsCollection
    {
        public TestPropertyIsCollection()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///取得或設定提供目前測試回合
        ///的相關資訊與功能的測試內容。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region 其他測試屬性

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext testContext)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        //
        // 您可以使用下列其他屬性撰寫您的測試:
        //
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在類別中的所有測試執行後，使用 ClassCleanup 執行程式碼
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在執行每一項測試之前，先使用 TestInitialize 執行程式碼
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在執行每一項測試之後，使用 TestCleanup 執行程式碼
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion 其他測試屬性

        [TestMethod]
        public void 驗證List集合是否能正常驗證Validation()
        {
            var father = new Father
            {
                Name = "Joey",
                Age = 999,
                Sons = new List<Son>
                {
                    new Son {Age = 0},
                    new Son {Name = string.Empty, Age = 200},
                    new Son {Name = "91", Age = 300},
                }
            };

            var validator = new DataAnnotationValidator();

            var isValid = validator.TryValidate(father);

            Assert.AreEqual(false, isValid);

            Assert.AreEqual(6, validator.ValidationResults.Count);

            Assert.AreEqual("The field Age must be between 1 and 130.", validator.ValidationResults[0].ErrorMessage);
            Assert.AreEqual("The field Sons[0].Age must be between 1 and 130.",
                validator.ValidationResults[1].ErrorMessage);
            Assert.AreEqual("The Sons[0].Name field is required.", validator.ValidationResults[2].ErrorMessage);
            Assert.AreEqual("The field Sons[1].Age must be between 1 and 130.",
                validator.ValidationResults[3].ErrorMessage);
            Assert.AreEqual("The Sons[1].Name field is required.", validator.ValidationResults[4].ErrorMessage);
            Assert.AreEqual("The field Sons[2].Age must be between 1 and 130.",
                validator.ValidationResults[5].ErrorMessage);
        }

        [TestMethod]
        public void 驗證ProductModel()
        {
            var productModel = new ProductModel
            {
                MyPerson = new Person { Id = 1, Name = string.Empty, Birthday = new DateTime(1991, 9, 1) },
                MyProduct = new Product { Cost = 100, SellPrice = 105 }
            };

            var validator = new DataAnnotationValidator();

            var isValid = validator.TryValidate(productModel);

            Assert.AreEqual(false, isValid);

            Assert.AreEqual(2, validator.ValidationResults.Count);
        }

        [TestMethod]
        public void 驗證GrandFather()
        {
            var grandFather = new GrandFather
            {
                Name = "hi",
                Fathers = new List<Father>
                {
                    new Father
                    {
                        Name = "Father1",
                        Age = 81,
                        Sons = new List<Son>
                        {
                            new Son {Name = "91", Age = 300},
                            new Son {Name = "92", Age = 11},
                            new Son {Name = "93", Age = 202},
                        }
                    },
                    new Father
                    {
                        Age = 82,
                        Sons = new List<Son>
                        {
                            new Son {Name = "F2_1", Age = -1},
                            new Son {Name = "F2_2", Age = -2},
                            new Son {Name = "F2_3", Age = 4},
                        }
                    },
                }
            };

            var validator = new DataAnnotationValidator();

            var isValid = validator.TryValidate(grandFather);

            Assert.AreEqual(false, isValid);

            Assert.AreEqual(7, validator.ValidationResults.Count);
        }

        [TestMethod]
        public void 驗證Sons_IEnumerable()
        {
            var father = new Father
            {
                Age = 90,
                Name = "Fat",
                Sons = Enumerable.Repeat<Son>(new Son { Name = "91", Age = 300 }, 2)
            };

            var validator = new DataAnnotationValidator();

            var isValid = validator.TryValidate(father);

            Assert.AreEqual(false, isValid);

            Assert.AreEqual(2, validator.ValidationResults.Count);
        }

        [TestMethod]
        public void 驗證Sons_IEnumerable為空_驗證為true()
        {
            var father = new Father
            {
                Age = 90,
                Name = "Fat",
            };

            var validator = new DataAnnotationValidator();

            var isValid = validator.TryValidate(father);

            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        public void 若model內集合都通過驗證_TryValidate應回傳true()
        {
            //// Arrange
            CallbackModel model = new CallbackModel();
            model.Items.Add(new CallbackModelItem()
            {
                SourceFilePath = "SourceFilePath",
                DestinationFilePath = "DestinationFilePath",
                Status = "Status",
                ErrorMessage = string.Empty
            });

            //// Act
            DataAnnotationValidator validator = new DataAnnotationValidator();
            bool isValid = validator.TryValidate(model);

            //// Assert
            Assert.IsTrue(isValid);
        }
    }
}