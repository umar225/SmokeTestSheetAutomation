import React, { useEffect, useState } from 'react';
import { useStripe } from '@stripe/react-stripe-js';
import Amplitude from '../utils/Amplitude';
import currencyFormatter from '../utils/currencyFormatter';

const updateamplitude = (courses) => {
  Amplitude('place order', {
    course: `${courses}`,
  });
};

const amplitudeError = (er) => {
  Amplitude('place order unsuccessful', {
    error: er,
    source: 'payment status page',
  });
};

const amplitudeInfo = (info) => {
  Amplitude('place order', {
    information: info,
    source: 'payment status page',
  });
};

export default function PayStatus() {
  const stripe = useStripe();

  const [message, setMessage] = useState(null);
  const [cartItems, setCartItems] = useState(null);
  const [cartTotal, setCartTotal] = useState(null);

  useEffect(() => {
    if (!stripe) {
      return;
    }

    const clientSecret = new URLSearchParams(window.location.search).get(
      'payment_intent_client_secret'
    );

    const paymentIntentId = new URLSearchParams(window.location.search).get(
      'payment_intent'
    );

    if (!clientSecret) {
      return;
    }

    if (!paymentIntentId) {
      return;
    }

    const checkStatus = (paymentIntent) => {
      const orderData = JSON.parse(localStorage.getItem('orderData'));
      let intentId;
      if (orderData?.intent) {
        intentId = orderData.intent;
      }
      if (intentId == paymentIntentId) {
        const cItems = JSON.parse(localStorage.getItem('finalBasket'));
        setCartItems(cItems);
        setCartTotal(paymentIntent.amount / 100);
        let coursesName = cItems.map(
          (value) => `${value.name} by ${value.provider}`
        );
        updateamplitude(coursesName);
        localStorage.removeItem('basketData');
        localStorage.removeItem('orderData');
        localStorage.removeItem('finalBasket');
      }
      setMessage('Payment succeeded!');
    };

    stripe.retrievePaymentIntent(clientSecret).then(({ paymentIntent }) => {
      switch (paymentIntent.status) {
        case 'succeeded':
          checkStatus(paymentIntent);
          break;
        case 'processing':
          setMessage('Your payment is processing.');
          amplitudeInfo('Your payment is processing.');
          break;
        case 'requires_payment_method':
          setMessage('Your payment was not successful, please try again.');
          amplitudeError(
            'Your payment was not successful, please try again. requires_payment_method'
          );
          break;
        default:
          setMessage('Something went wrong.');
          amplitudeError('Something went wrong.');
          break;
      }
    });
  }, [stripe]);

  return (
    <div className="whiteBackground bottomPadding50 topPadding50">
      {/* Show any error or success messages */}
      {message && (
        <div id="payment-message" className="">
          <h3>{message}</h3>
        </div>
      )}
      {cartItems && (
        <div className="stripeF row bottom50">
          <div className="col-1 col-md-1"></div>
          <div className="col-10 col-md-10">
            <h3>Your order</h3>
            <table>
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tbody>
                {cartItems.map((item) => (
                  <tr key={item.id}>
                    <td>
                      <tr style={{ border: 'none', fontFamily: 'Arvo' }}>
                        {item.name} <strong>× {item.quantity}</strong>
                      </tr>
                      <tr style={{ fontSize: '12px', border: 'none' }}>
                        {item.provider}
                      </tr>
                    </td>
                    <td>{currencyFormatter.format(item.totalPrice)}</td>
                  </tr>
                ))}
              </tbody>
              {cartTotal && (
                <tfoot>
                  <tr>
                    <th>Subtotal</th>
                    <td>
                      <b>{currencyFormatter.format(cartTotal)}</b>
                    </td>
                  </tr>
                  <tr>
                    <th>Total</th>
                    <td>
                      <b>{currencyFormatter.format(cartTotal)}</b>
                    </td>
                  </tr>
                </tfoot>
              )}
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
