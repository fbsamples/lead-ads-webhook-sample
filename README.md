# Lead Ads Webhook Sample on AWS

This is a sample code (C# ASP.NET Core) for Facebook's Webhook and Lead Ads's products, focused on retrieving the leads. This sample code was also used in an internal training here at Facebook, as a demo session. You also have a Postman collection in the `postman` folder for testing purposes.

This repo is related to the (C#) REST webservice module (Back-End), leveraging AWS SAM accelerator. I believe this can be easily ported to other cloud providers, because the focus is on the webhook's endpoints.

Based on the [ASP.NET Core]( https://docs.microsoft.com/en-us/dotnet/api/?view=aspnetcore-3.1 ) Web API Serverless Application template.

Created by M. França - Solutions Architect (Solutions Engineering team LATAM).

## TL;DR;

Get this source code...
```
git clone
```

Install the AWS templates and tools for your VS Code installation...
```
dotnet new --install Amazon.Lambda.Templates
dotnet tool install –g Amazon.Lambda.Tools
```

Retrieve the project's dependencies, compile everything, and run it locally...
```
dotnet restore
dotnet run
```

Validate the SAM yaml file, then package everything, and finally deploy it to the AWS cloud...
```
sam validate --template-file aws_iaac.yaml
dotnet lambda package --output-package .aws-sam\build\MyAWSomeLambda\prjFBLeadAds.zip
sam deploy --guided --template-file aws_iaac.yaml
```

Monitor the logs to find the id of your new lead...
```
sam logs -n MyAWSomeLambda --stack-name fra-fbleadads-back-stk --tail --region sa-east-1
```

## Setup

### If you want to create it from scratch...

```
dotnet new serverless.AspNetCoreWebAPI --name prjFBLeadAds –o .
dotnet restore
```

### Testing Locally (Running)

```
dotnet run
```

### Package & Deploy (on AWS)...

```
dotnet lambda package --output-package .aws-sam\build\MyAWSomeLambda\prjFBLeadAds.zip
sam deploy --guided --template-file aws_iaac.yaml
```

### Decomissioning

```
aws cloudformation delete-stack --stack-name sam-app --region region
```

## References

* [Facebook Webhooks]( https://developers.facebook.com/docs/graph-api/webhooks/getting-started )
* [Retrieving Leads API]( https://developers.facebook.com/docs/marketing-api/guides/lead-ads/retrieving )
* [AWS Command Line Interface]( https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-welcome.html )
* [AWS Serverless Application Model (SAM)]( https://aws.amazon.com/serverless/sam/ )
* [Postman for testing API calls]( https://www.postman.com/ )

## Additional Info

### License

This source code is licensed under the MIT License. See the `LICENSE` file.

### Contributions

This project is waiting for your contribution. See the `CONTRIBUTING.md` file.

### Contact

Marcelo França - www.linkedin.com/in/mafranca