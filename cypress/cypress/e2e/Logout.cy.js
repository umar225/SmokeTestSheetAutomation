describe.only('Logout', () => {
    it('Logout success', () => {
      cy.visit('https://app.d.getboardwise.com/login')
      cy.get("#email").type("umar.javed@fusiontech.global")
      cy.get("#email").should("have.value", "umar.javed@fusiontech.global")
      cy.get("#password").type("Liverpool@1")
      cy.get("#password").should("have.value", "Liverpool@1")
      cy.get("#loginBtn").click()
      cy.url().should("include","/userdashboard")
      cy.wait(100);
      cy.get('#collasible-nav-dropdown').click()
      cy.get('.dropdown-menu > :nth-child(4)').click()
    })
})