class redirect{
    
    home_index(){
        wx.switchTab({
            url: '/pages/home/index/index',
          });
    }

}

module.exports={
    redirect:new redirect()
}