import { checkoutApi } from "./checkoutApi";


const createOrUpdateOrder = order => async dispatch => {
  const data = await checkoutApi.createOrUpdateOrder(order);
  return data;
};

const getIntentId = intentId => async dispatch => {
  const data = await checkoutApi.getIntentId(intentId);
  return data;
};

const getSubsciptionIntentId = (productId, priceId) => async () => {
  const data = await checkoutApi.getSubsciptionIntentId(productId, priceId);
  return data;
}

const createSubscription = (email, priceId, paymentMethodId) => async () => {
  const data = await checkoutApi.createSubscription(email, priceId, paymentMethodId);
  return data;
}

export const checkoutActions = {
  createOrUpdateOrder: createOrUpdateOrder,
  getIntentId: getIntentId,
  getSubsciptionIntentId: getSubsciptionIntentId,
  createSubscription: createSubscription,
};
