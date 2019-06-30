import { post } from '../http/http.js'
import { api } from '../http/api.js'
import { setting } from '../common/setting.js'
import { cache } from '../common/cache.js'
import { redirect } from '../common/redirect.js'

class helper {
    post(param) {return post(param)}
    api() {return api}
    setting() {return setting}
    cache() {return cache}
    redirect() {return redirect}
}

module.exports = {
    hp:new helper()
}