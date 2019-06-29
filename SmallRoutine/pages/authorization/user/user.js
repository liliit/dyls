var app = getApp();
import { post } from '../../../utils/http/http.js'
import { api } from '../../../utils/http/api.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    height: app.globalData.height
  },

  bindGetUserInfo: function (e) {
    console.log(app.globalData.login)
    console.log(e)
    
    //用户授权成功
    if (e.detail.errMsg == "getUserInfo:ok") {
      app.globalData.userInfo = e.detail.rawData;
    }

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.login({
      success: res => {
        post({
          url: api.login,
          data: { code: res.code },
        }).then(res => {
          if (res.data.code === 0) {

          }
        })
      }
    });
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