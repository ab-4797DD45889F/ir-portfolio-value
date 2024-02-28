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
