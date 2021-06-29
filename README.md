# concurrency-test

#### script to run concurrent requests (used apache benchmark command):
```bash
 ab -n 1 -c 1 <request_url>
```

#### api endpoints
```
http://localhost:5000/buy/{productId}
http://localhost:5000/getquantity/{productId}
```
