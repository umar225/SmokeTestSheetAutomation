import React, { useState } from "react";
import { PopupButton } from "@typeform/embed-react";
import { useDispatch } from "react-redux";
import { homeActions } from "../store/home/homeActions";

function TestScreen(props) {
  const dispatch = useDispatch();
  const [courses, setCourses] = useState([]);
  const onSubmitForm = value => {
    dispatch(homeActions.onGetTFResponse(value)).then(data => {
      if (data && data.length > 0) {
        setCourses(data);
      } else {
        setCourses([]);
      }
    });
  };
  return (
    <div>
      <PopupButton
        id="RKadH2mY"
        onSubmit={event => {
          onSubmitForm(event.responseId);
        }}
        autoClose={true}
        style={{ fontSize: 20 }}
        className="my-button"
      >
        click to open form in popup
      </PopupButton>
      {courses && courses.length > 0 && courses[0].name ? (
        <div>
          <p>List of courses are here:</p>
          {courses.map((course) => (
            <div key={course.name} style={{ display: "flex", flexDirection: "row" }}>
              <p>Name:</p>
              <p>{course.name ? course.name : course}</p>
            </div>
          ))}
        </div>
      ) : (
        <p>{courses}</p>
      )}
    </div>
  );
}
export default TestScreen;
