# ir-portfolio-value

Requests your portfolio (a set of accounts/wallets) from the IR account and estimates its value as if you sold out everything with market sell orders.  It differs from the value that you can see at your dashboard, because the latter is based on the last trade price, which isn't exactly what you will get if you want to sell everything at once.  

You need to generate an api key here https://portal.independentreserve.com/settings/api-keys and use it in the request below.

:warning: **WARNING**: Use only `Read Only` api key type. 

```
$ docker run -it --rm 
    ghcr.io/ab-4797dd45889f/ir-portfolio-value:latest \
    --key aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee \
    --secret xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx \
    --currency Aud
```

## Sample Output

```
Requesting valid primary currencies... Xbt, Eth, Sol, Xrp, Usdc, Usdt, Aave, Ada, Bat, Bch, Comp, Dai, Doge, Dot, Eos, Etc, Grt, Link, Ltc, Mana, Matic, Mkr, Sand, Snx, Uni, Xlm, Yfi, Zrx
Requesting valid secondary currencies... Aud, Usd, Nzd, Sgd
Requesting accounts... 45 accounts received
Requesting FX rates... 30 FX rates received
FX: 2.0 Usd -> 3.05 Aud (rate: 1.5233)
Requesting Xbt order book... 526 sell orders received
Requesting Eth order book... 455 sell orders received
Requesting Sol order book... 327 sell orders received
Requesting Xrp order book... 576 sell orders received
Requesting Usdt order book... 224 sell orders received
Requesting Ada order book... 353 sell orders received
Requesting Bch order book... 121 sell orders received
Requesting Doge order book... 252 sell orders received
Requesting Dot order book... 205 sell orders received
Omg 0.615737683357038694 doesn't look like supported primary or secondary currency, ignoring
Requesting Zrx order book... 178 sell orders received

Portfolio as of (6/28/2025 1:36:11 PM +06:00):

 $1,285.49 | Xbt 0.00781041
   $908.58 | Eth 0.244244469493919099
   $553.60 | Sol 2.48129
   $446.66 | Xrp 133.26091
   $295.48 | Ada 342.65614
   $256.20 | Doge 1027.69553
     $6.27 | Dot 1.2
     $3.05 | Usd 2.0
     $0.71 | Bch 0.0009
     $0.00 | Usdt 0.000009
     $0.00 | Zrx 0.0000054
     $0.00 | Omg 0.615737683357038694

 $3,756.04 | Total
```


## Contribute

You can easily take part in the development and improvement of this project yourself with VS code and devcontaners.

```
$ git clone https://github.com/ab-4797DD45889F/ir-portfolio-value.git
$ cd ir-portfolio-value
$ code . 
```

- Assuming you already use VS Code, install `Dev Containers` extension
- Use the Command Palette (Ctrl + Shift + P) and choose `Dev Containers: Rebuild and Reopen in container`
- Once the project gets reload in container, open the vscode terminal and run the following commands to start the application

```
$ dotnet restore
$ dotnet run -- --key aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee --secret xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx --currency Aud
```

## Build sources and run in docker

```
$ docker build -t ir-portfolio-value .
$ docker run -it --name ir-portfolio-value ir-portfolio-value --key aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee --secret xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx --currency Aud
```

## Publish docker image

Personal access token (PAT) with `write:packages` and `read:packages` privileges is required.

```
$ echo TOKEN | docker login ghcr.io -u USERNAME --password-stdin 
```

```
$ git clean -fXd
$ docker build -t ghcr.io/ab-4797dd45889f/ir-portfolio-value:latest .
$ docker push ghcr.io/ab-4797dd45889f/ir-portfolio-value:latest
```
