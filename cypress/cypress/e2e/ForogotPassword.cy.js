describe('ForgotPassword', () => {
    it('ForgotPassword Succesfully', () => {
      cy.visit('https://app.d.getboardwise.com/')
      cy.get('.rememberWrapper > :nth-child(1)').click()
      cy.get("#email").type("umar.javed@fusiontech.global")
      cy.get("#email").should("have.value", "umar.javed@fusiontech.global")
      cy.get('.loginTxt').click()
      cy.get('.backToLogin').click()
    })
  })