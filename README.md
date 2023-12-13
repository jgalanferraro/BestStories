# Best Stories API

## Overview

API based on ASP.NET Core. It takes care of handling requests to Hacker News API and returning the top n best stories.

## Design

It implements the mediator pattern to decouple the logic from the Controller and uses In-memory cache to avoid overcalling the Hacker News API.

The Controller on the other hand implements two endpoints to retrieve the requested data. 

The first one uses a basic url /api/beststories/{top} where top is an integer that define the number of stories we want to retrieve.

The second one uses OData to allow a $top query. This could be used like /api/beststories?$top=n. Please take into account only this type of query is implemented and any other will trigger an error.

## Items

A single story is serialized as it was defined in the exercise description.

```javascript
{
"title" : "A uBlock Origin update was rejected from the Chrome Web Store",
"uri" : "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
"postedBy" : "ismaildonmez",
"time" : "2019-10-12T13:43:01+00:00",
"score" : 1716,
"commentCount" : 572
}
```


## Assumptions

The request to https://hacker-news.firebaseio.com/v0/beststories.json already returns the stories ids sorted by score, so no logic is implemented for sorting, instead I have implemented an integration test that proves it.

## Improvements

The main one would be optimazing the caching strategy, maybe with a deph research of the data stability in order to get the right times the data should stay in the cache and also subscribing to the firebase service in order to get the updates before the user request them.
IF scalability was needed, distributed cache would replace the in-memory cache and the api would need to integrate with a event bus to connect with other services.

## How to use it


