# RestSharp.Configuration
Read request information from a **har** file, and create a new **RestRequest**().

So that we can login to some web pages without writing **Request.AddParameter()** over and over.

# Steps: #

## 1. Get a Har file  
Save a real request from Chrome as HAR format.

## 2. Write the following code ##
RestClient restClient = new RestClient("https://www.yourUrl.com");

IList<RestRequest> request = ConfigureHelper.SetConfigure(MockHarFile);

restClient.Execute(request.First());

# Project Introduction: #

## 1. RestSharp.Configuration ##

(1) The method SetConfigure creates a **RestRequest** based on a **har** file.

(2) Use StyleCop to keep code clean.

## 2. RestSharp.Configuration.Test ##
(1) Use **NUnit** to test.
