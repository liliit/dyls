class cache{

    set(key,value){
        wx.setStorageSync(key, value);
    }

    get(key)
    {
        return wx.getStorageSync(key);
    }
}

module.exports={
    cache:new cache()
}