import amplitude from 'amplitude-js';

function Amplitude(event, props, userId) {
  let AmplitudeKey = window.env?.AMPLITUDE_KEY;

  amplitude.getInstance().init(AmplitudeKey);
  if (userId) {
    amplitude.getInstance().setUserId(userId);
  } else {
    amplitude.getInstance().setUserId(null);
  }
  amplitude.getInstance().logEvent(event, props);
}

export default Amplitude;
