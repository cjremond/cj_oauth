# cj_oauth
利用.NET Framework4.0基于各平台OAuth2.0的接口，实现了钉钉、支付宝、微信、QQ、微博、百度的扫码登录三方。

其中钉钉三方登录满足企业用户的限定，需要是企业内部用户；支付宝参考源码进行自主签名，没有使用支付宝集成的d。

后续的扫码登录对接也会更新，如果针对某一个需要对接，可以在issues中提出来。

### 配置文件样例

#### 1.钉钉
```
app_id = "dingoatbyu*********",
app_secret = "sM6JcqesK1hoe6jY*********************************",
scope = "snsapi_login",
dingcorp_appkey = "dingxlkle*********",    //企业应用相关
dingcorp_appsecret = "zYz8oMOhQ3vm89R***************************",
```
#### 2.支付宝
```
app_id = "202100********",
scope = "auth_user,auth_base",

支付宝需要涉及到一个私钥文件，保存到根目录AliKeyPems下，命名为RSA2_private_key.txt，你可以根据自身的实际情况修改
```
#### 3.微信
```
app_id = "wxaf7742c********",  //应用的AppKey
app_secret = "11b3c6f15e0191****************",
scope = "snsapi_login",
```
#### 4.QQ
```
app_id = "1019********",
app_secret = "8ad61935************************",
scope = "get_user_info",

QQ 如果要获取unionid，先去申请https://wiki.connect.qq.com/unionid%E4%BB%8B%E7%BB%8D
只需要配置参数`withUnionid= true`，默认不会请求获取 Unionid

```
#### 5.新浪微博
```
app_id = "285********",  //应用的AppKey
app_secret = "f7bacdedeed************************",
```
#### 6.百度
```
app_id = "VL1uIBT****************",  //应用的AppKey
app_secret = "kCgER48QI1V9************************",
```


### 返回样例
```
{    
        "OpenId":"o_nyV5g2Xh482bDzkIWYaZQdzJR8",    
        "UnionId":"oIw7Kw-I3qyoGYOfzxih4zN3Jq6I",    
        "Channel":"weixin",    
        "NickName":"雷馒头",    
        "Gender":"m", 
        "Avatar":"https://thirdwx.qlogo.cn/mmopen/v********Mg0OKFggJgBoUg/132"
 }
```


### 其他
使用中如果有什么问题，请提交 issue，我会及时查看

或者加我微信![image](https://github.com/cjremond/cj_oauth/blob/main/001.jpg)
