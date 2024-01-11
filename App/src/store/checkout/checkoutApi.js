import { axios } from "../../config/axiosConfig";
let rootURL = window.env?.URL;
async function createOrUpdateOrder(data) {
  return axios
  .post(`${rootURL}payment`, data)
  .then(response => {    
    let data = response.data;
    return data;
  })
  .catch(error => {    
    return error;
  });
}

async function getIntentId(intentId) {
  return axios
  .post(`${rootURL}payment/get/intent`, intentId)
  .then(response => {   
    let data = response.data;
    return data;
  })
  .catch(error => {    
    return error;
  });
}

async function getSubsciptionIntentId(productId, priceId) {
  return axios
  .post(`${rootURL}payment/get/SubscriptionIntent`, productId, priceId)
  .then(response => {
    let data = response.data;
    return data;
  })
  .catch(error => {    
    return error;
  });
}
async function createSubscription(email, priceId, paymentMethodId) {
  return axios
  .post(`${rootURL}payment/createSubscription`, email, priceId, paymentMethodId)
  .then(response => {
    let data = response.data;
    return data;
  })
  .catch(error => {    
    return error;
  });
}

export const checkoutApi = {
  createOrUpdateOrder: createOrUpdateOrder,
  getIntentId:getIntentId,
  getSubsciptionIntentId: getSubsciptionIntentId,
  createSubscription: createSubscription,
};
