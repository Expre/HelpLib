# HelpLib日常开发帮助类库使用文档（持续更新中）
# Crypto

 1. MD5Helper：计算 MD5 哈希值
 2. SHAHelper：SHA1、SHA256 、SHA384 、SHA512 哈希值
 3. Base64Helper：BASE64加密/解密
 4. DESHelper：DES加密/解密
 5. RSAHelper：RSA（创建密钥/加密/解密/原数据哈希值/数字签名/签名验证）

## MD5Helper
 1. `GetHash`：计算字符串MD5哈希值；
 2. `GetHash`：计算流MD5哈希值；
 3. `GetHashBytes`：计算字符串MD5哈希值；
 4. `GetHashBytes`：计算流MD5哈希值；
 5. `GetPassword`：获取加盐密码；

## HttpHelper
 1. `Post`：模拟post请求；
 2. `Get`：模拟get请求；
 3. `GetImageStream`：获取远程地址的图片流；
 4. `PostFile`：向远程地址post多个文件以及键值数据；
 5. `PostFile`：向远程地址post单个文件以及键值数据；
 6. `ClientPostFile`：使用HttpClient模拟form表单提交键值和本地多个文件；
