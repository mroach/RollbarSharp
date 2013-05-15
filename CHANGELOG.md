## 0.1.6.0 (2013-05-15)

* Solved threading problem.
* When an X-Forwarded-For HTTP header exists, use that as the user's IP address rather than REMOTE_ADDR. When a user is behind a proxy server or load balancer, X-Forwarded-For will be their actual IP address.
* Simplified the exception filter example
* Removed testing key from config


## 0.1.5.0 (2013-04-26)

* Added properties 'machine' and 'software' to the 'server' model. These aren't offically supported but they show up on the web and they're useful.
* Changed Custom from an object to Dictionary<string, object>.
* Now automatically copying the Exception.Data dictionary to Custom['exception_data'].
* Renamed RollbarClient.DataBuilder to NoticeBuilder. A more friendly name, I think
* For text noticed, changed dictionary key type from 'string' to 'object' to make it easier to use
* Dump server vars in MVC test. Add custom data to the exception we raise.


## 0.1.4.0 (2013-04-24)

* Added RequestStarting event to RollbarClient so you can log the JSON posted to Rollbar


## 0.1.3.0 (2013-04-24)

* Include "input type=file" elements in the POST parameters collection


## 0.1.2.0 (2013-04-23)

* Fixed json.net dependency version


## 0.1.1.0 (2013-04-23)

* Added web.config transform
* Ignore null appsetting values


## 0.1.0.0 (2013-04-23)

* Initial release. Testing out nuget