const age = 55;

const salary = 35000;

if((age>=18 && age<=60)&& salary >=5000){
    let max=60000;
    let loanAmount=salary*3;
    if(loanAmount>max){
        console.log(`Congratulations! You are eligible for 60000 EGP loan`);
    }
    else{
        console.log(`Congratulations! You are eligible for ${loanAmount} EGP loan`);  
    }
}
else{
    console.log(`Applicant is rejected because they do not meet the minimum requirements`);

}