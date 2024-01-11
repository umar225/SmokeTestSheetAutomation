describe('Registration', () => {
  it('click on Registration Button', () => {
      cy.wait(500)
      cy.visit('https://app.d.getboardwise.com/login')
      cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/span[2]/span').click()
      cy.url().should("include","/registeruser")
  })

  it.only('Enter Email,Pass,CPass,Fname,Lname', () => {
    cy.wait(500)
    cy.visit('https://app.d.getboardwise.com/registeruser')
    cy.get("#email").type("umar.javed+1999@fusiontech.global")
    cy.get("#email").should("have.value", "umar.javed+1999@fusiontech.global")
    cy.get("#password").type("Liverpool@1")
    cy.get("#password").should("have.value", "Liverpool@1")
    cy.get("#password2").type("Liverpool@1")
    cy.get("#password2").should("have.value", "Liverpool@1")
    cy.get("#firstName").type("Umar")
    cy.get("#firstName").should("have.value","Umar")
    cy.get("#lastName").type("Javed")
    cy.get("#lastName").should("have.value","Javed")
    cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
})

it.only('Failure case 1: Enter Email Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#email").type("umar.javed+1999@fusiontech.global")
  cy.get("#email").should("have.value", "umar.javed+1999@fusiontech.global")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 2:Enter Password Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#password").type("Liverpool@1")
  cy.get("#password").should("have.value", "Liverpool@1")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 3:Enter confirm Password Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#password2").type("Liverpool@1")
  cy.get("#password2").should("have.value", "Liverpool@1")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 4:Enter First Name Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#firstName").type("Umar")
  cy.get("#firstName").should("have.value","Umar")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 5:Enter Last Name Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#lastName").type("Javed")
  cy.get("#lastName").should("have.value","Javed")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 6: Enter Email & Password Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#email").type("umar.javed+1999@fusiontech.global")
  cy.get("#email").should("have.value", "umar.javed+1999@fusiontech.global")
  cy.get("#password").type("Liverpool@1")
  cy.get("#password").should("have.value", "Liverpool@1")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 7: Enter Email & CPassword Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#email").type("umar.javed+1999@fusiontech.global")
  cy.get("#email").should("have.value", "umar.javed+1999@fusiontech.global")
  cy.get("#password2").type("Liverpool@1")
  cy.get("#password2").should("have.value", "Liverpool@1")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})

it.only('Failure case 8:Enter First Name & Last Name Only', () => {
  cy.wait(500)
  cy.visit('https://app.d.getboardwise.com/registeruser')
  cy.get("#firstName").type("Umar")
  cy.get("#firstName").should("have.value","Umar")
  cy.get("#lastName").type("Javed")
  cy.get("#lastName").should("have.value","Javed")
  cy.xpath('/html/body/div[1]/div[1]/div/div/div/div/div/div[2]/div/form/div[6]/button').click()
  cy.get(".errorTxt")
})
})
//comment
//comment