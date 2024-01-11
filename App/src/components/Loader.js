import React from "react";
import Spinner from "react-bootstrap/Spinner";

function Loader() {
  return (
    <div className="loaderWrapper">
      <Spinner animation="border" />
    </div>
  );
}

export default Loader;
