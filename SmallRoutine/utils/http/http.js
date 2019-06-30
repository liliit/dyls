
var app = getApp();
import md5 from '../common/md5.js'

export const post=(param={})=>{
    wx.showLoading({
        title: "加载中"
      })
    return new Promise(function(resolve,reject){

        wx.request({
            url:app.globalData.api+param.url,
            data: param.data,
            method: 'POST',
            header: header(),
            success:res=>{
                wx.hideLoading();

                if(res.data.code==0)
                {
                    resolve(res.data);
                }
                else
                {
                    
                }
            }
        })

    })

}

export const header=(param={})=>{
     //时间戳
  let time = timesTamp()
  //key
  let key = app.globalData.appletkey
  //appid
  let appid = app.globalData.appid
  //signature
  let signature = md5.hexMD5(time + key + appid);

  let header = {
    "DYLS-Applet": "Agent-Remote",
    "X-Scanner-Ud": "Addr",
    timesTamp: time,
    signature: signature
  }
  return header
}

export const timesTamp = (param = {}) => {
    return Date.parse(new Date())
  }