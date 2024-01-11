describe.only('AdminLogin', () => {
    it('Admin Login success', () => {
      cy.visit('https://app.d.getboardwise.com/admin/login')
      cy.get("#email").type("admin@getcoursewise.com")
      cy.get("#email").should("have.value", "admin@getcoursewise.com")
      cy.get("#password").type("efsV94qDrBnDGZfA")
      cy.get("#password").should("have.value", "efsV94qDrBnDGZfA")
      cy.get('.loginBtn').click()
      cy.url().should("include","/admindashboard")
    })
    it('Admin Login Failure1', () => {
        cy.visit('https://app.d.getboardwise.com/admin/login')
        cy.get("#email").type("admin1@getcoursewise.com")
        cy.get("#email").should("have.value", "admin1@getcoursewise.com")
        cy.get("#password").type("efsV94qDrBnDGZfA")
        cy.get("#password").should("have.value", "efsV94qDrBnDGZfA")
        cy.get('.loginBtn').click()
        cy.get('.login-modal')
      })
      it('Admin Login Failure2', () => {
        cy.visit('https://app.d.getboardwise.com/admin/login')
        cy.get("#email").type("admin@getcoursewise.com")
        cy.get("#email").should("have.value", "admin@getcoursewise.com")
        cy.get("#password").type("efsV9dada4qDrBnDGZfA")
        cy.get("#password").should("have.value", "efsV9dada4qDrBnDGZfA")
        cy.get('.loginBtn').click()
        cy.get('.login-modal')
      })
      //test
})