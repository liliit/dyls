var app = getApp();
import { hp } from '../../../utils/helper/helper.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    height: app.globalData.height,
    loading: true
  },

  Login: function () {
    wx.login({
      success: res => {
        hp.post({
          url: hp.api().login,
          data: { code: res.code },
        }).then(res => {
          if (res.code === 0) {
            hp.cache().set(hp.setting().login, res.data);
            this.setData({
              loading: false
            })
          }
        })
      }
    });
  },

  bindGetUserInfo: function (e) {
    //用户授权成功
    if (e.detail.errMsg == "getUserInfo:ok") {

      hp.cache.set(hp.setting().userInfo, e.detail.rawData)
      var userInfo = JSON.parse(e.detail.rawData);
      var login = hp.cache().get(hp.setting().login);

      var obj = {
        applet_OpenId: login.openid,
        unionid: login.unionid
      }

      var model = Object.assign(userInfo, obj);

      hp.post({
        url: hp.api().userinfo,
        data: model,
      }).then(res => {
        hp.cache.set(hp.setting().user, res.data)
      })
    }

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var login = hp.cache().get(hp.setting().login);
    var userInfo = hp.cache().get(hp.setting().userInfo)
    var user = hp.cache().get(hp.setting().user)
    this.Login();
    if (login && userInfo && user) {
      hp.redirect().home_index();
    }
    else {
      
    }
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})