---
title: Questionnaire
created: '2021-11-03T22:33:31.375Z'
modified: '2021-11-03T22:54:48.594Z'
---

# Questionnaire

<h1>Limitations and Shortcuts</h1>

* Hard-coded website refresh time and website urls
* Direct commits to the master branch. For real-world projects, I will create feature branches and unit tests, before creating a pull request to the develop/master branch.

<h1>Over-designed features</h1>

* Pricing data from external websites is refreshed as a background task rather than being triggered every time the page is refreshed.

<h1>Scalability</h1>

* As data is refreshed at fixed intervals, the performance of the system is not impacted significantly as the number of requests increases. If response time is an issue, multiple instances of this solution can be deployed and connected using a load-balancer. In addition, this solution can be easily deployed to Azure, which supports dynamic-scaling and brings the service closer to clients around the world. 

<h1>Future enhancements</h1>

* More robust error handling and error logging
* Read hard-coded values from config files
* Better UI
* Unit and system tests
