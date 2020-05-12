Public Class Form1
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        ' hardcoded variables first for testing

        'validator and price parameters
        Dim ETHPrice As Decimal = txtPriceUSD.Text
        Dim perValidatorDeposit = 32
        Dim totalNumberOfETH = textBox1.Text
        Dim numberOfValidators = textBox2.Text
        Dim totalStaked As Decimal = txtTotalStaked.Text
        Dim avgNetworkOnline = txtNetworkUptime.Text
        Dim validatorUpTime = txtUpTime.Text

        'network parameters
        Dim shardCount = 64
        Dim slotTime = 12 'seconds
        Dim SlotsPerEpoch = 32
        Dim epochsPerYear = (60 * 60 * 24 * 365.242) / (slotTime * SlotsPerEpoch) 'adjusted for leap years

        'reward and penalty parameters
        Dim baseRewardFactor = 64 'Base Reward Factor
        Dim baseRewardsPerEpoch = 4
        Dim baseRewardProportionalValidatorsOnline = 3
        Dim inclusionPenaltyForOneSlotLate = 1 / 64

        'calculate base reward for full validator
        Dim baseRewardForFullValidator = 32 * (10 ^ 9) * baseRewardFactor / Math.Sqrt(totalStaked * (10 ^ 9)) / baseRewardsPerEpoch

        'calculate online per-validator reward per epoch
        Dim onlinePerValidatorRewardPerEpoch = baseRewardForFullValidator * baseRewardProportionalValidatorsOnline * avgNetworkOnline _
                                                + 1 * (0.125 * baseRewardForFullValidator * avgNetworkOnline + 0.875 * baseRewardForFullValidator *
                                                (avgNetworkOnline + avgNetworkOnline * (1 - avgNetworkOnline) * (1 - inclusionPenaltyForOneSlotLate) +
                                                avgNetworkOnline * (1 - avgNetworkOnline) ^ 2 * (1 - 2 * inclusionPenaltyForOneSlotLate)))

        'calculate offline per-validator penalty per epoch
        Dim offlinePerValidatorPenaltyPerEpoch = baseRewardForFullValidator * 4
        Dim stakingInterest = (onlinePerValidatorRewardPerEpoch * validatorUpTime - offlinePerValidatorPenaltyPerEpoch * (1 - validatorUpTime)) * epochsPerYear _
                                / 10 ^ 9 / perValidatorDeposit


        'tabulate for 5 years
        txtPrinciple.Text = ETHPrice * totalNumberOfETH
        txt1Year.Text = Math.Round(totalNumberOfETH * (1 + stakingInterest) * ETHPrice, 2) - stakingCost.Text
        txt2Years.Text = Math.Round(txt1Year.Text * (1 + stakingInterest), 2) - stakingCost.Text
        txt3Years.Text = Math.Round(txt2Years.Text * (1 + stakingInterest), 2) - stakingCost.Text
        txt4Years.Text = Math.Round(txt3Years.Text * (1 + stakingInterest), 2) - stakingCost.Text
        txt5Years.Text = Math.Round(txt4Years.Text * (1 + stakingInterest), 2) - stakingCost.Text

    End Sub

    Private Sub textBox2_TextChanged(sender As Object, e As EventArgs) Handles textBox2.TextChanged
        If IsNumeric(textBox2.Text) Then
            textBox1.Text = textBox2.Text * 32
        End If
    End Sub
End Class
