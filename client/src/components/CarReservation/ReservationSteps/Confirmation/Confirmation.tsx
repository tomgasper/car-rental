"use client"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"
import type { ReservationFormData } from "@/lib/types"
import { isPersonalInfoValid } from "@/lib/utils/reservation"
import { PersonalInfoForm } from "./PersonalInfoForm"
import { PriceBreakdown } from "./PriceBreakdown"
import { ReservationSummary } from "./ReservationSummary"

interface ConfirmationProps {
  formData: ReservationFormData
  onInputChange: (field: keyof ReservationFormData, value: any) => void
  onBack: () => void
  onConfirm: () => void
  isLoading: boolean
}

export function Confirmation({ formData, onInputChange, onBack, onConfirm, isLoading }: ConfirmationProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Reservation Summary</CardTitle>
        <CardDescription>Review your reservation details and enter your information</CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <ReservationSummary formData={formData} />

        <Separator />

        <PriceBreakdown formData={formData} />

        <Separator />

        <PersonalInfoForm formData={formData} onChange={onInputChange} />
      </CardContent>
      <CardFooter className="flex justify-between">
        <Button variant="outline" onClick={onBack}>
          Back
        </Button>
        <Button onClick={onConfirm} disabled={isLoading || !isPersonalInfoValid(formData)}>
          {isLoading ? "Processing..." : "Confirm Reservation"}
        </Button>
      </CardFooter>
    </Card>
  )
}
