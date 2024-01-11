describe('Dashboard', () => {
  it('Open Dashboard', () => {
    cy.visit('https://app.d.getboardwise.com/')
    cy.get("#email").type("umar.javed@fusiontech.global")
    cy.get("#email").should("have.value", "umar.javed@fusiontech.global")
    cy.wait(500)
    cy.get("#password").type("Liverpool@1")
    cy.get("#password").should("have.value", "Liverpool@1")
    cy.wait(500)
    cy.get("#loginBtn").click()
    cy.wait(500)
    cy.url().should("include","/userdashboard")
    //trying
  })
})