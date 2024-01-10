# ir-portfolio-value

Requests your portfolio (a set of accounts/wallets) from the IR account and estimates portfolio value as if you sold out everything with market sell orders.

Please use only api keys with read-only privileges. 

## Sample Output

```
Requesting valid primary currencies... Xbt, Eth, Xrp, Usdc, Usdt, Aave, Ada, Bat, Bch, Comp, Dai, Doge, Dot, Eos, Etc, Grt, Link, Ltc, Mana, Matic, Mkr, Sand, Snx, Sol, Uni, Xlm, Yfi, Zrx
Requesting valid secondary currencies... Aud, Usd, Nzd, Sgd
Requesting accounts... 39 accounts received
Requesting Xbt order book... 300 sell orders received
Requesting Eth order book... 238 sell orders received
Requesting Usdt order book... 71 sell orders received
Requesting Bch order book... 108 sell orders received
Omg 0.615737683357038694 doesn't look like supported primary or secondary currency, ignoring
Requesting Sol order book... 97 sell orders received
Requesting Zrx order book... 80 sell orders received

Portfolio as of (1/10/2024 10:19:22 AM +06:00):

 $2,754.72 | Xbt 0.03992454
 $1,383.75 | Eth 0.392836059493919099
   $329.72 | Aud 329.72
   $147.12 | Sol 0.98629
    $76.60 | Usdt 51.273469
     $0.33 | Bch 0.0009
     $0.00 | Zrx 0.0000054
     $0.00 | Omg 0.615737683357038694

 $4,692.25 | Total
```


## To Do

- Move config to command line arguments
- Wrap it to docker, so that we could run this command from docker environment
