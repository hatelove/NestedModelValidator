#our user story
Feature: PropertyValidation
	In order to 使用Attribute就能針對entity進行Validation動作
	As a 開發人員
	I want to 取得Validation失敗的property name, error message, value

# Scenario means acceptance test cases
# design test cases by scenario, by domain specfic language, all from user requirement
Scenario: 驗證Person的Name必須有值，若null或空字串，則驗證結果失敗，取得對應的error message
	Given 針對Person
	| Id | Name | Birthday   |
	| 1  |      | 1991-09-01 |		
	When 呼叫DataAnnotationValidator的TryValidate方法
	Then 應回傳 false 
	And ValidationResult應為 1 筆，其PropertyName為 Name ，其ErrorMessage應為 The Name field is required. 
	
		
Scenario Outline:  驗證Customer的Age必須介於18~100，若Age不在此範圍中，則驗證結果失敗，取得對應的error message
Given Customer的ID為<id>
And Customer的Age為<age>
When 呼叫DataAnnotationValidator的TryValidate方法
Then 應回傳 <expectResult>
And ValidationResult應為 <errorCount> 筆，其PropertyName為 <propertyName> ，其ErrorMessage應為 <errorMessage>

Examples: 
| id | age | expectResult | errorCount | propertyName | errorMessage                             |
| 1  | 10  | false        | 1          | Age          | The field Age must be between 18 and 80. |
| 2  | 18  | true         | 0          |              |                                          |

Scenario Outline: 驗證Order上的Customer，若Age不在18~100，則驗證結果失敗
Given Order的ID為<orderId>
And Customer的ID為<id> 
And Customer的Age為<age> 
When 呼叫DataAnnotationValidator的TryValidate方法
Then 應回傳 <expectResult>
And ValidationResult應為 <errorCount> 筆，其PropertyName為 <propertyName> ，其ErrorMessage應為 <errorMessage>

Examples: 
| orderId | id | age | expectResult | errorCount | propertyName                 | errorMessage                                                                                     |
| 10      | 3  | 10  | false        | 2          | MyCustomer.Id,MyCustomer.Age | The field MyCustomer.Id must be between 0 and 2.,The field MyCustomer.Age must be between 18 and 80. |
| 20      | 2  | 18  | true         | 0          |                              |                                                                                                 |

Scenario: 驗證三層Model，是否可正確取得Validation錯誤訊息
Given OrderGroup的OrderName為空
And MyOrder的Id為110
And MyOrder的MyCustomer的Age為10
And MyOrder的MyCustomer的Id為1
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName             | ErrorMessage                                                |
	| OrderName              | The OrderName field is required.                            |
	| MyOrder.Id             | The field MyOrder.Id must be between 0 and 100.             |
	| MyOrder.MyCustomer.Age | The field MyOrder.MyCustomer.Age must be between 18 and 80. |

@product
Scenario: 驗證Product自訂商業邏輯，是否可正確取得Validation錯誤訊息
Given Product為
	| Cost | SellPrice |
	| 100  | 105       |
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName | ErrorMessage |
	| 毛利率        | 毛利率需大於6%     |

@product
Scenario: 驗證Product的Cost與SellPrice不符合驗證規則時，應只先檢查property，而不檢查自訂商業邏輯
Given Product為
	| Cost | SellPrice |
	| 50   | 51        |
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName | ErrorMessage                                            |
	| Cost       | The field Cost must be between 100 and 2147483647.      |
	| SellPrice  | The field SellPrice must be between 100 and 2147483647. |	                                             

@productModel
Scenario: 驗證ProductModel中，Person property與Product property不符合驗證規則時，是否可正確取得Validation錯誤訊息
Given ProductModel.MyPerson為
	| Id | Name | Birthday   |
	| 1  |      | 1991-09-01 |
And ProductModel.MyProduct為
	| Cost | SellPrice |
	| 50   | 51        |
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName          | ErrorMessage                                                      |
	| MyPerson.Name       | The MyPerson.Name field is required.                              |
	| MyProduct.Cost      | The field MyProduct.Cost must be between 100 and 2147483647.      |
	| MyProduct.SellPrice | The field MyProduct.SellPrice must be between 100 and 2147483647. |

@productModel
Scenario: 驗證ProductModel中，Person property與Product property皆符合驗證規則時，是否可正確驗證Product自訂商業邏輯
Given ProductModel.MyPerson為
	| Id | Name   | Birthday   |
	| 1  | "Joey" | 1991-09-01 |
And ProductModel.MyProduct為
	| Cost | SellPrice |
	| 100  | 105       |
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName    | ErrorMessage       |
	| MyProduct.毛利率 | MyProduct.毛利率需大於6% |

@productModel
Scenario: 驗證ProductModel中，Person property不符合驗證，但Product property皆符合驗證規則時，是否可正確驗證Product自訂商業邏輯
Given ProductModel.MyPerson為
	| Id | Name | Birthday |
	| 1  |      | 1991-09-01 |
And ProductModel.MyProduct為
	| Cost | SellPrice |
	| 100  | 105       |
When 呼叫DataAnnotationValidator的TryValidate方法
Then  TryValidate結果應為 false 
And 應回傳ValidationResults
	| MemberName    | ErrorMessage                         |
	| MyPerson.Name | The MyPerson.Name field is required. |
	| MyProduct.毛利率 | MyProduct.毛利率需大於6%                   |