import { useEffect } from "react"
import type { ReservationFormData } from "@/lib/types"
import { InitialReservationForm } from "./ReservationSteps/InitialReservationForm/InitialReservationForm"
import { Confirmation } from "./ReservationSteps/Confirmation/Confirmation"
import { Complete } from "./ReservationSteps/Complete/Complete"
import { useReservationForm } from "@/hooks/useReservationForm"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { AlertCircle } from "lucide-react"

// form steps
const STEPS = {
    RESERVATION_FORM: 0,
    AVAILABILITY_CONFIRMATION: 1,
    RESERVATION_COMPLETE: 2,
  }

export default function ReservationForm() {
  const {
    formData,
    step,
    isLoading,
    error,
    reservationId,
    handleInputChange,
    handleCheckAvailability,
    handleConfirmReservation,
    handleBack,
    handleReset,
    setStep,
  } = useReservationForm()

  // listen for popstate events (browser back/forward buttons)
  useEffect(() => {
    const handlePopState = () => {
      const searchParams = new URLSearchParams(window.location.search)
      const stepParam = searchParams.get("step")
      if (stepParam) {
        const parsedStep = Number.parseInt(stepParam, 10)
        if (!isNaN(parsedStep) && parsedStep >= 0 && parsedStep <= 2) {
          
          setStep(parsedStep)
          return
        }
      }
      setStep(STEPS.RESERVATION_FORM)
    }

    window.addEventListener("popstate", handlePopState)
    return () => {
      window.removeEventListener("popstate", handlePopState)
    }
  }, [])

  return (
    <div className="flex flex-col justify-center items-center container-sm mx-auto max-w-3xl py-8 gap-2">
      {error && (
        <Alert variant="destructive" className="break-words px-2">
          <AlertCircle className="h-4 w-4 shrink-0" />
          <AlertDescription className="w-full">{error}</AlertDescription>
        </Alert>
      )}

      {renderStep(step, {
        formData,
        isLoading,
        reservationId,
        onInputChange: handleInputChange,
        onCheckAvailability: handleCheckAvailability,
        onConfirm: handleConfirmReservation,
        onBack: handleBack,
        onReset: handleReset,
      })}
    </div>
  )
}

function renderStep(
  step: number,
  props: {
    formData: ReservationFormData
    isLoading: boolean
    reservationId: string
    onInputChange: (field: keyof ReservationFormData, value: any) => void
    onCheckAvailability: () => Promise<void>
    onConfirm: () => Promise<void>
    onBack: () => void
    onReset: () => void
  }
) {
  switch (step) {
    case STEPS.RESERVATION_FORM:
      return (
        <InitialReservationForm
          formData={props.formData}
          onInputChange={props.onInputChange}
          onCheckAvailability={props.onCheckAvailability}
          isLoading={props.isLoading}
        />
      )
    case STEPS.AVAILABILITY_CONFIRMATION:
      return (
        <Confirmation
          formData={props.formData}
          onInputChange={props.onInputChange}
          onBack={props.onBack}
          onConfirm={props.onConfirm}
          isLoading={props.isLoading}
        />
      )
    case STEPS.RESERVATION_COMPLETE:
      return <Complete formData={props.formData} reservationId={props.reservationId} onReset={props.onReset} />
    default:
      return null
  }
}