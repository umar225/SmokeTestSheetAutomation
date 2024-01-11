// describe('Apply Job', () => {
//   it('Apply Job', () => {
//     cy.visit('https://app.d.getboardwise.com/')
//     cy.get("#email").type("umar.javed@fusiontech.global")
//     cy.get("#email").should("have.value", "umar.javed@fusiontech.global")
//     cy.wait(500)
//     cy.get("#password").type("Liverpool@1")
//     cy.get("#password").should("have.value", "Liverpool@1")
//     cy.wait(500)
//     cy.get("#loginBtn").click()
//     cy.wait(500)
//     cy.url().should("include","/userdashboard")
//     cy.wait(500)
//     cy.get('.jobsBoardDisplay > :nth-child(1)').click()
//     cy.get('.applyButtonStyle').click()
//     cy.wait(500)
    
//     //Applying for Job filling the detials

//     cy.get("#name").type("Umar Javed")
//     cy.get("#name").should("have.value","Umar Javed")
//     cy.wait(500)
//     cy.get("#email").type("umar.javed@fusiontech.global")
//     cy.get("#email").should("have.value","umar.javed@fusiontech.global")
//     cy.wait(500)
//     cy.get("#valueToAdd").type("Positive")
//     cy.get("#valueToAdd").should("have.value","Positive")
//     cy.wait(500)
//     cy.get('input[name="myCV"][type="file"][accept=".pdf, .doc, .docx"][style="display: none;"]').attachFile('sample_cv.pdf');
//     cy.wait(500)
//     cy.get('.applyJobModalBody > .applyButtonStyle').click()
//     cy.wait(500)
//   });
    
//   })
  