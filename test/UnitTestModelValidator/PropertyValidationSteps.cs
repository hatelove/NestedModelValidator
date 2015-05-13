using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Infra.ModelValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using UnitTestModelValidator.ModelForValidation;

namespace UnitTestModelValidator
{
    [Binding]
    public class PropertyValidationSteps
    {
        public static DataAnnotationValidator target;

        [Scope(Feature = "PropertyValidation")]
        [BeforeScenario]
        public static void BeforeFeature()
        {
            target = new DataAnnotationValidator();
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Person的Name必須有值，若null或空字串，則驗證結果失敗，取得對應的error message")]
        [Given(@"針對Person")]
        public void Given針對Person(Table table)
        {
            var person = table.CreateInstance<Person>();

            ScenarioContext.Current.Set<Person>(person);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Person的Name必須有值，若null或空字串，則驗證結果失敗，取得對應的error message")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法()
        {
            var person = ScenarioContext.Current.Get<Person>();

            bool result = target.TryValidate(person);

            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }

        [Scope(Feature = "PropertyValidation")]
        [Then(@"應回傳 (.*)")]
        public void Then應回傳False(bool expectResult)
        {
            var result = ScenarioContext.Current.Get<bool>("result");
            Assert.AreEqual(expectResult, result);
        }

        [Scope(Feature = "PropertyValidation")]
        [Then(@"ValidationResult應為 (.*) 筆，其PropertyName為 (.*) ，其ErrorMessage應為 (.*)")]
        public void ThenValidationResult應為筆其PropertyName為Name其ErrorMessage應為Xxx(int errorCount, string propertyName, string errorMessage)
        {
            var validationResults = ScenarioContext.Current.Get<ICollection<ValidationResult>>().ToList();

            Assert.AreEqual(errorCount, validationResults.Count);

            if (string.IsNullOrEmpty(propertyName) || errorCount == 0)
            {
                return;
            }

            var properties = propertyName.Split(',');
            var errorMessages = errorMessage.Split(',');

            for (int i = 0; i < properties.Length; i++)
            {
                Assert.AreEqual(properties[i], validationResults[i].MemberNames.FirstOrDefault());
                Assert.AreEqual(errorMessages[i], validationResults[i].ErrorMessage);
            }
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Customer的Age必須介於18~100，若Age不在此範圍中，則驗證結果失敗，取得對應的error message")]
        [Given(@"Customer的ID為(.*)")]
        public void GivenCustomer的ID為(int id)
        {
            var customer = new Customer();
            customer.Id = id;

            ScenarioContext.Current.Set<Customer>(customer);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Customer的Age必須介於18~100，若Age不在此範圍中，則驗證結果失敗，取得對應的error message")]
        [Given(@"Customer的Age為(.*)")]
        public void GivenCustomer的Age為(int age)
        {
            var customer = ScenarioContext.Current.Get<Customer>();
            customer.Age = age;
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Customer的Age必須介於18~100，若Age不在此範圍中，則驗證結果失敗，取得對應的error message")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法_customer()
        {
            var customer = ScenarioContext.Current.Get<Customer>();
            var result = target.TryValidate(customer);

            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Order上的Customer，若Age不在18~100，則驗證結果失敗")]
        [Given(@"Order的ID為(.*)")]
        public void GivenOrder的ID為(int id)
        {
            var order = new Order { Id = id, MyCustomer = new Customer() };

            ScenarioContext.Current.Set<Order>(order);
            ScenarioContext.Current.Set<Customer>(order.MyCustomer);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Order上的Customer，若Age不在18~100，則驗證結果失敗")]
        [Given(@"Customer的ID為(.*)")]
        public void GivenCustomer的ID為_order(int id)
        {
            var customer = ScenarioContext.Current.Get<Customer>();
            customer.Id = id;

            ScenarioContext.Current.Set<Customer>(customer);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Order上的Customer，若Age不在18~100，則驗證結果失敗")]
        [Given(@"Customer的Age為(.*)")]
        public void GivenCustomer的Age為_order(int age)
        {
            var customer = ScenarioContext.Current.Get<Customer>();
            customer.Age = age;
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證Order上的Customer，若Age不在18~100，則驗證結果失敗")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法_order()
        {
            var order = ScenarioContext.Current.Get<Order>();

            var result = target.TryValidate(order);

            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證三層Model，是否可正確取得Validation錯誤訊息")]
        [Given(@"OrderGroup的OrderName為空")]
        public void GivenOrderGroup的OrderName為空()
        {
            var orderGroup = new OrderGroup { OrderName = string.Empty };
            ScenarioContext.Current.Set<OrderGroup>(orderGroup);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證三層Model，是否可正確取得Validation錯誤訊息")]
        [Given(@"MyOrder的Id為(.*)")]
        public void GivenMyOrder的Id為(int myOrderId)
        {
            var orderGroup = ScenarioContext.Current.Get<OrderGroup>();
            orderGroup.MyOrder = new Order { Id = myOrderId };

            ScenarioContext.Current.Set<OrderGroup>(orderGroup);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證三層Model，是否可正確取得Validation錯誤訊息")]
        [Given(@"MyOrder的MyCustomer的Age為(.*)")]
        public void GivenMyOrder的MyCustomer的Age為(int myCustomerAge)
        {
            var orderGroup = ScenarioContext.Current.Get<OrderGroup>();
            orderGroup.MyOrder.MyCustomer = new Customer { Age = myCustomerAge };

            ScenarioContext.Current.Set<OrderGroup>(orderGroup);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證三層Model，是否可正確取得Validation錯誤訊息")]
        [Given(@"MyOrder的MyCustomer的Id為(.*)")]
        public void GivenMyOrder的MyCustomer的Id為(int myCustomerId)
        {
            var orderGroup = ScenarioContext.Current.Get<OrderGroup>();
            orderGroup.MyOrder.MyCustomer.Id = myCustomerId;

            ScenarioContext.Current.Set<OrderGroup>(orderGroup);
        }

        [Scope(Feature = "PropertyValidation", Scenario = "驗證三層Model，是否可正確取得Validation錯誤訊息")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法_OrderGroup()
        {
            var orderGroup = ScenarioContext.Current.Get<OrderGroup>();

            var result = target.TryValidate(orderGroup);

            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }

        [Then(@"TryValidate結果應為 (.*)")]
        public void ThenTryValidate結果應為(bool expected)
        {
            var actual = ScenarioContext.Current.Get<bool>("result");

            Assert.AreEqual(expected, actual);
        }

        [Then(@"應回傳ValidationResults")]
        public void Then應回傳ValidationResults(Table table)
        {
            var validationResults = ScenarioContext.Current.Get<ICollection<ValidationResult>>().ToList();

            Assert.AreEqual(table.RowCount, validationResults.Count);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.AreEqual(table.Rows[i]["MemberName"], validationResults[i].MemberNames.FirstOrDefault() ?? string.Empty);
                Assert.AreEqual(table.Rows[i]["ErrorMessage"], validationResults[i].ErrorMessage);
            }
        }

        [Scope(Tag = "product")]
        [Given(@"Product為")]
        public void GivenProduct為(Table table)
        {
            var product = table.CreateInstance<Product>();
            ScenarioContext.Current.Set<Product>(product);
        }

        [Scope(Tag = "product")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法_product()
        {
            var product = ScenarioContext.Current.Get<Product>();
            var result = target.TryValidate(product);

            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }

        [Scope(Tag = "productModel")]
        [Given(@"ProductModel\.MyPerson為")]
        public void GivenProductModel_MyPerson為(Table table)
        {
            var productModel = new ProductModel();

            var person = table.CreateInstance<Person>();
            productModel.MyPerson = person;

            ScenarioContext.Current.Set<ProductModel>(productModel);
        }

        [Scope(Tag = "productModel")]
        [Given(@"ProductModel\.MyProduct為")]
        public void GivenProductModel_MyProduct為(Table table)
        {
            var productModel = ScenarioContext.Current.Get<ProductModel>();

            var product = table.CreateInstance<Product>();
            productModel.MyProduct = product;
        }

        [Scope(Tag = "productModel")]
        [When(@"呼叫DataAnnotationValidator的TryValidate方法")]
        public void When呼叫DataAnnotationValidator的TryValidate方法_productModel()
        {
            var productModel = ScenarioContext.Current.Get<ProductModel>();

            var result = target.TryValidate(productModel);
            ScenarioContext.Current.Set<ICollection<ValidationResult>>(target.ValidationResults);
            ScenarioContext.Current.Set<bool>(result, "result");
        }
    }
}