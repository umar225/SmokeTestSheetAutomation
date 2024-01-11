import React, { useEffect, useState } from 'react';
import {
  PaymentElement,
  useStripe,
  useElements,
} from '@stripe/react-stripe-js';
import myEnv from '../utils/env.json';
import Amplitude from '../utils/Amplitude';
import currencyFormatter from '../utils/currencyFormatter';
import PropTypes from 'prop-types';

export default function CheckoutForm(props) {
  const stripe = useStripe();
  const elements = useElements();
  const cartItems = props.cartItems;
  const [message, setMessage] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [basketTotal, setBasketTotal] = React.useState(0);

  useEffect(() => {
    let total = 0.0;
    cartItems.map((item) => {
      total = item.totalPrice + total;
    });
    setBasketTotal(total);
  }, []);

  const amplitudeError = (er) => {
    Amplitude('place order unsuccessful', {
      error: er,
      source: 'checkout page',
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!stripe || !elements) {
      // Stripe.js has not yet loaded.
      // Make sure to disable form submission until Stripe.js has loaded.
      return;
    }
    const billingDetails = {
      name: e.target.billingInputName.value,
      email: e.target.billingInputEmail.value,
      phone: e.target.billingInputPhone.value,
      address: {
        city: e.target.billingInputCity.value,
        line1: e.target.billingInputAddress.value,
        line2: e.target.billingInputAddressTwo.value,
        postal_code: e.target.billingInputZip.value,
        state: e.target.billingInputCounty.value,
        country: e.target.billingInputCountry.value,
      },
    };

    setIsLoading(true);
    fetch(`${myEnv.URL}payment/customer`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: e.target.billingInputName.value,
        email: e.target.billingInputEmail.value,
        number: e.target.billingInputPhone.value,
        companyName: e.target.billingInputCompany.value,
        notes: e.target.billingInputNotes.value,
        intentId: props.intentId,
        address: {
          city: e.target.billingInputCity.value,
          line1: e.target.billingInputAddress.value,
          line2: e.target.billingInputAddressTwo.value,
          postalCode: e.target.billingInputZip.value,
          state: e.target.billingInputCounty.value,
          country: e.target.billingInputCountry.value,
        },
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        if (!data.success) {
          amplitudeError(data.message);
          setMessage('An unexpected error occured');
        }
      });

    const { error } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        // Make sure to change this to your payment completion page
        return_url: `${window.location.origin}/paymentStatus`,
        payment_method_data: {
          billing_details: billingDetails,
        },
      },
    });

    // This point will only be reached if there is an immediate error when
    // confirming the payment. Otherwise, your customer will be redirected to
    // your `return_url`. For some payment methods like iDEAL, your customer will
    // be redirected to an intermediate site first to authorize the payment, then
    // redirected to the `return_url`.
    if (error.type === 'card_error' || error.type === 'validation_error') {
      amplitudeError(error.message);
      setMessage(error.message);
    } else {
      amplitudeError(error.message);
      setMessage('An unexpected error occured.');
    }

    setIsLoading(false);
  };

  const options = {
    fields: {
      billingDetails: 'never',
    },
  };

  return (
    <form id="payment-form" className="row" onSubmit={handleSubmit}>
      <div className="row">
        <div className="col-0 col-md-1"></div>
        <div className="col-12 col-md-5">
          <div className="row top50">
            <h3>Billing details</h3>
            <div className="form-group">
              <label htmlFor="billingInputName">
                Name <span className="text-danger">*</span>
              </label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputName"
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="billingInputCompany">
                Company name (Optional)
              </label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputCompany"
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputCountry">
                Country/Region <span className="text-danger">*</span>
              </label>
              <select
                className="custom-select"
                id="billingInputCountry"
                defaultValue="GB"
                style={{ width: '100%' }}
              >
                <option disabled="" value="">
                  Select
                </option>
                <option value="AF">Afghanistan</option>
                <option value="AX">Åland Islands</option>
                <option value="AL">Albania</option>
                <option value="DZ">Algeria</option>
                <option value="AD">Andorra</option>
                <option value="AO">Angola</option>
                <option value="AI">Anguilla</option>
                <option value="AQ">Antarctica</option>
                <option value="AG">Antigua &amp; Barbuda</option>
                <option value="AR">Argentina</option>
                <option value="AM">Armenia</option>
                <option value="AW">Aruba</option>
                <option value="AU">Australia</option>
                <option value="AT">Austria</option>
                <option value="AZ">Azerbaijan</option>
                <option value="BS">Bahamas</option>
                <option value="BH">Bahrain</option>
                <option value="BD">Bangladesh</option>
                <option value="BB">Barbados</option>
                <option value="BY">Belarus</option>
                <option value="BE">Belgium</option>
                <option value="BZ">Belize</option>
                <option value="BJ">Benin</option>
                <option value="BM">Bermuda</option>
                <option value="BT">Bhutan</option>
                <option value="BO">Bolivia</option>
                <option value="BA">Bosnia &amp; Herzegovina</option>
                <option value="BW">Botswana</option>
                <option value="BV">Bouvet Island</option>
                <option value="BR">Brazil</option>
                <option value="IO">British Indian Ocean Territory</option>
                <option value="VG">British Virgin Islands</option>
                <option value="BN">Brunei</option>
                <option value="BG">Bulgaria</option>
                <option value="BF">Burkina Faso</option>
                <option value="BI">Burundi</option>
                <option value="KH">Cambodia</option>
                <option value="CM">Cameroon</option>
                <option value="CA">Canada</option>
                <option value="CV">Cape Verde</option>
                <option value="BQ">Caribbean Netherlands</option>
                <option value="KY">Cayman Islands</option>
                <option value="CF">Central African Republic</option>
                <option value="TD">Chad</option>
                <option value="CL">Chile</option>
                <option value="CN">China</option>
                <option value="CO">Colombia</option>
                <option value="KM">Comoros</option>
                <option value="CG">Congo - Brazzaville</option>
                <option value="CD">Congo - Kinshasa</option>
                <option value="CK">Cook Islands</option>
                <option value="CR">Costa Rica</option>
                <option value="CI">Côte d’Ivoire</option>
                <option value="HR">Croatia</option>
                <option value="CW">Curaçao</option>
                <option value="CY">Cyprus</option>
                <option value="CZ">Czechia</option>
                <option value="DK">Denmark</option>
                <option value="DJ">Djibouti</option>
                <option value="DM">Dominica</option>
                <option value="DO">Dominican Republic</option>
                <option value="EC">Ecuador</option>
                <option value="EG">Egypt</option>
                <option value="SV">El Salvador</option>
                <option value="GQ">Equatorial Guinea</option>
                <option value="ER">Eritrea</option>
                <option value="EE">Estonia</option>
                <option value="SZ">Eswatini</option>
                <option value="ET">Ethiopia</option>
                <option value="FK">Falkland Islands</option>
                <option value="FO">Faroe Islands</option>
                <option value="FJ">Fiji</option>
                <option value="FI">Finland</option>
                <option value="FR">France</option>
                <option value="GF">French Guiana</option>
                <option value="PF">French Polynesia</option>
                <option value="TF">French Southern Territories</option>
                <option value="GA">Gabon</option>
                <option value="GM">Gambia</option>
                <option value="GE">Georgia</option>
                <option value="DE">Germany</option>
                <option value="GH">Ghana</option>
                <option value="GI">Gibraltar</option>
                <option value="GR">Greece</option>
                <option value="GL">Greenland</option>
                <option value="GD">Grenada</option>
                <option value="GP">Guadeloupe</option>
                <option value="GU">Guam</option>
                <option value="GT">Guatemala</option>
                <option value="GG">Guernsey</option>
                <option value="GN">Guinea</option>
                <option value="GW">Guinea-Bissau</option>
                <option value="GY">Guyana</option>
                <option value="HT">Haiti</option>
                <option value="HN">Honduras</option>
                <option value="HK">Hong Kong SAR China</option>
                <option value="HU">Hungary</option>
                <option value="IS">Iceland</option>
                <option value="IN">India</option>
                <option value="ID">Indonesia</option>
                <option value="IQ">Iraq</option>
                <option value="IE">Ireland</option>
                <option value="IM">Isle of Man</option>
                <option value="IL">Israel</option>
                <option value="IT">Italy</option>
                <option value="JM">Jamaica</option>
                <option value="JP">Japan</option>
                <option value="JE">Jersey</option>
                <option value="JO">Jordan</option>
                <option value="KZ">Kazakhstan</option>
                <option value="KE">Kenya</option>
                <option value="KI">Kiribati</option>
                <option value="XK">Kosovo</option>
                <option value="KW">Kuwait</option>
                <option value="KG">Kyrgyzstan</option>
                <option value="LA">Laos</option>
                <option value="LV">Latvia</option>
                <option value="LB">Lebanon</option>
                <option value="LS">Lesotho</option>
                <option value="LR">Liberia</option>
                <option value="LY">Libya</option>
                <option value="LI">Liechtenstein</option>
                <option value="LT">Lithuania</option>
                <option value="LU">Luxembourg</option>
                <option value="MO">Macao SAR China</option>
                <option value="MG">Madagascar</option>
                <option value="MW">Malawi</option>
                <option value="MY">Malaysia</option>
                <option value="MV">Maldives</option>
                <option value="ML">Mali</option>
                <option value="MT">Malta</option>
                <option value="MQ">Martinique</option>
                <option value="MR">Mauritania</option>
                <option value="MU">Mauritius</option>
                <option value="YT">Mayotte</option>
                <option value="MX">Mexico</option>
                <option value="MD">Moldova</option>
                <option value="MC">Monaco</option>
                <option value="MN">Mongolia</option>
                <option value="ME">Montenegro</option>
                <option value="MS">Montserrat</option>
                <option value="MA">Morocco</option>
                <option value="MZ">Mozambique</option>
                <option value="MM">Myanmar (Burma)</option>
                <option value="NA">Namibia</option>
                <option value="NR">Nauru</option>
                <option value="NP">Nepal</option>
                <option value="NL">Netherlands</option>
                <option value="NC">New Caledonia</option>
                <option value="NZ">New Zealand</option>
                <option value="NI">Nicaragua</option>
                <option value="NE">Niger</option>
                <option value="NG">Nigeria</option>
                <option value="NU">Niue</option>
                <option value="MK">North Macedonia</option>
                <option value="NO">Norway</option>
                <option value="OM">Oman</option>
                <option value="PK">Pakistan</option>
                <option value="PS">Palestinian Territories</option>
                <option value="PA">Panama</option>
                <option value="PG">Papua New Guinea</option>
                <option value="PY">Paraguay</option>
                <option value="PE">Peru</option>
                <option value="PH">Philippines</option>
                <option value="PN">Pitcairn Islands</option>
                <option value="PL">Poland</option>
                <option value="PT">Portugal</option>
                <option value="PR">Puerto Rico</option>
                <option value="QA">Qatar</option>
                <option value="RE">Réunion</option>
                <option value="RO">Romania</option>
                <option value="RU">Russia</option>
                <option value="RW">Rwanda</option>
                <option value="WS">Samoa</option>
                <option value="SM">San Marino</option>
                <option value="ST">São Tomé &amp; Príncipe</option>
                <option value="SA">Saudi Arabia</option>
                <option value="SN">Senegal</option>
                <option value="RS">Serbia</option>
                <option value="SC">Seychelles</option>
                <option value="SL">Sierra Leone</option>
                <option value="SG">Singapore</option>
                <option value="SX">Sint Maarten</option>
                <option value="SK">Slovakia</option>
                <option value="SI">Slovenia</option>
                <option value="SB">Solomon Islands</option>
                <option value="SO">Somalia</option>
                <option value="ZA">South Africa</option>
                <option value="GS">
                  South Georgia &amp; South Sandwich Islands
                </option>
                <option value="KR">South Korea</option>
                <option value="SS">South Sudan</option>
                <option value="ES">Spain</option>
                <option value="LK">Sri Lanka</option>
                <option value="BL">St. Barthélemy</option>
                <option value="SH">St. Helena</option>
                <option value="KN">St. Kitts &amp; Nevis</option>
                <option value="LC">St. Lucia</option>
                <option value="MF">St. Martin</option>
                <option value="PM">St. Pierre &amp; Miquelon</option>
                <option value="VC">St. Vincent &amp; Grenadines</option>
                <option value="SR">Suriname</option>
                <option value="SJ">Svalbard &amp; Jan Mayen</option>
                <option value="SE">Sweden</option>
                <option value="CH">Switzerland</option>
                <option value="TW">Taiwan</option>
                <option value="TJ">Tajikistan</option>
                <option value="TZ">Tanzania</option>
                <option value="TH">Thailand</option>
                <option value="TL">Timor-Leste</option>
                <option value="TG">Togo</option>
                <option value="TK">Tokelau</option>
                <option value="TO">Tonga</option>
                <option value="TT">Trinidad &amp; Tobago</option>
                <option value="TA">Tristan da Cunha</option>
                <option value="TN">Tunisia</option>
                <option value="TR">Turkey</option>
                <option value="TM">Turkmenistan</option>
                <option value="TC">Turks &amp; Caicos Islands</option>
                <option value="TV">Tuvalu</option>
                <option value="UG">Uganda</option>
                <option value="UA">Ukraine</option>
                <option value="AE">United Arab Emirates</option>
                <option value="GB">United Kingdom</option>
                <option value="US">United States</option>
                <option value="UY">Uruguay</option>
                <option value="UZ">Uzbekistan</option>
                <option value="VU">Vanuatu</option>
                <option value="VA">Vatican City</option>
                <option value="VE">Venezuela</option>
                <option value="VN">Vietnam</option>
                <option value="WF">Wallis &amp; Futuna</option>
                <option value="EH">Western Sahara</option>
                <option value="YE">Yemen</option>
                <option value="ZM">Zambia</option>
                <option value="ZW">Zimbabwe</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="billingInputAddress">
                Address <span className="text-danger">*</span>
              </label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputAddress"
                placeholder="House number and street name"
                required
              />
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputAddressTwo"
                placeholder="Apartment, suite, unit, etc. (optional)"
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputCity">
                City <span className="text-danger">*</span>
              </label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputCity"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputCounty">County (Optional)</label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputCounty"
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputZip">
                Postcode <span className="text-danger">*</span>
              </label>
              <input
                type="text"
                className="form-control stripeF"
                id="billingInputZip"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputPhone">
                Phone <span className="text-danger">*</span>
              </label>
              <input
                type="tel"
                className="form-control stripeF"
                id="billingInputPhone"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="billingInputEmail">
                Email address <span className="text-danger">*</span>
              </label>
              <input
                type="email"
                className="form-control stripeF"
                id="billingInputEmail"
                aria-describedby="emailHelp"
                required
              />
            </div>
          </div>
          <div className="row top50">
            <h3>Additional information</h3>
            <div className="form-group" style={{ marginBottom: '5px' }}>
              <label htmlFor="billingInputNotes">Order notes (optional)</label>
              <textarea
                type="text"
                className="form-control stripeF"
                id="billingInputNotes"
                placeholder="Notes about your order, e.g. special notes for delivery."
              />
            </div>
          </div>
        </div>

        <div className="col-12 col-md-5"></div>
      </div>

      <div className="row top50">
        <div className="col-0 col-md-1"></div>
        <div className="col-12 col-md-5">
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
            <tfoot>
              <tr>
                <th>Subtotal</th>
                <td>
                  <b>{currencyFormatter.format(basketTotal)}</b>
                </td>
              </tr>
              <tr>
                <th>Total</th>
                <td>
                  <b>{currencyFormatter.format(basketTotal)}</b>
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>

      <div className="row top15">
        <div className="col-0 col-md-1"></div>
        <div className="col-12 col-md-5">
          <div className="stripeBackground">
            <PaymentElement id="payment-element" options={options} />
            <div className="row stripePersonalData">
              <p>
                Your personal data will be used to process your order, support
                your experience throughout this website, and for other purposes
                described in our{' '}
                <a
                  href="https://getcoursewise.com/privacy-policy/"
                  target="_blank"
                >privacy policy</a>.
              </p>
            </div>
            <div className="row">
              <div className="col-0 col-sm-6"></div>
              <div className="col-12 col-sm-6">
                <button
                  disabled={isLoading || !stripe || !elements}
                  id="submit"
                  className="buttonStyle"
                >
                  <span id="button-text">
                    {isLoading ? (
                      <div className="spinner" id="spinner"></div>
                    ) : (
                      'Place Order'
                    )}
                  </span>
                </button>
              </div>
            </div>

            {/* Show any error or success messages */}
            {message && <div id="payment-message">{message}</div>}
          </div>
        </div>
      </div>
    </form>
  );
}
CheckoutForm.propTypes = {
  cartItems: PropTypes.array,
  intentId: PropTypes.string,
};