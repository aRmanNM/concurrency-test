# concurrency-test

#### script to run concurrent requests (used apache benchmark command):
```bash
 ab -n 1 -c 1 <request_url>
```

#### api endpoints:
```
http://localhost:5000/buy/{productId}
http://localhost:5000/getquantity/{productId}
```

#### usefull links:
[https://github.com/madelson/DistributedLock](https://github.com/madelson/DistributedLock)

[https://www.codemag.com/article/0607081/Database-Concurrency-Conflicts-in-the-Real-World](https://www.codemag.com/article/0607081/Database-Concurrency-Conflicts-in-the-Real-World)

[https://redis.io/topics/distlock](https://redis.io/topics/distlock)

[https://github.com/samcook/RedLock.net](https://github.com/samcook/RedLock.net)

[https://martin.kleppmann.com/2016/02/08/how-to-do-distributed-locking.html](https://martin.kleppmann.com/2016/02/08/how-to-do-distributed-locking.html)

[http://antirez.com/news/101](http://antirez.com/news/101)

