import Vue from 'vue'
import Vuex from 'vuex'

import {authModule as auth} from 'sun'
import {categoriesModule as categories} from 'sun'
import {menuModule as menu} from 'sun'
import {adminModule as admin} from 'sun'
import {rootModule as root} from 'sun'

import {setStore} from 'sun'


Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

export default function (/* { ssrContext } */) {

  const store = new Vuex.Store({
    ...root,
    modules: {
      admin,
      auth,
      categories,
      menu
    }
  });

  setStore(store);
  store.dispatch("initUserFromLocalStorage");

  return store;
}





