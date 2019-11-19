## Validators

Validators are the design pattern **Chain of responsibility** implementation used for complex validation.
To create a complex validator you must follow the steps:

##### Create validation provider:

Your first step is to make an interface and following class for your validation provider that contains the method which will be used for your validation:

```
public interface ICustomValidationProvider
{
    ValidationResult ValidateSomething(object data);
}

public class CustomValidationProvider : ICustomValidationProvider 
{
    public ValidationResult ValidateSomething(object data)
    {
        // validation logic
    }
}
```
You can use **ValidationResult** class that contain **Success** and **Message** which might be use in your validation callback.

##### Create validation handlers:

The pattern used into validators split rules of validation into chain handlers. Each handler contains specific validation logic. To create a validation handler you have to use AbstractHandler<T> where T is the object which you will transfer between chain handlers.

```
public class RuleOneHandler : AbstractHandler<object>
{
    protected override string HandleProcessAction()
    {
        // validation logic for rule one
        // set this.requestObject to null in case of invalid object
        // return message from the validation
    }
}
```

##### Connect validation rules in chain:

When validation provider and its validation rules are available you must make the validation chain by using the following structure:

```
public ValidationResult ValidateSomething(object data)
{
    var startupHandler = new StartupHandler<object>();
    var ruleOneHandler = new RuleOneHandler();
    var ruleTwoHandler = new RuleTwoHandler();
    var ruleThreeHandler = new RuleThreeHandler();

    startupHandler
        .SetNext(ruleOneHandler)
        .SetNext(ruleTwoHandler)
        .SetNext(ruleThreeHandler);

    ValidationResult validationResult = new ValidationResult();
    string resultMessage = string.Empty;
    validationResult.Success = startupHandler.Handle(data, out resultMessage) != null;
    validationResult.Message = resultMessage;
    if (validationResult.Success)
    {
        validationResult.Message = "Everything is fine.";
    }

    return validationResult;
}
```

A real example is available on [GitHub Repository](https://github.com/intellisoft567/mvc567/tree/master/src/Mvc567.Services/Validators).