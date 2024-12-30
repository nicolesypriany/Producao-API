import { Button } from "@/components/ui/button";
import { Field } from "@/components/ui/field";
import { useCreateMachine } from "@/machine/hooks/useCreateMachine";
import { Flex, Heading, Input } from "@chakra-ui/react";

const CreateMachine = () => {
  const { handleSubmit, onSubmit, errors, register } = useCreateMachine();

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Flex p={4} direction="column" gap={5}>
        <Flex w="100%" justifyContent="center">
          <Heading fontSize="4xl">Cadastro de máquinas</Heading>
        </Flex>

        <Flex direction="column" gap={4}>
          <Field
            label="Nome da máquina"
            invalid={!!errors.machineName}
            errorText={errors.machineName?.message}
          >
            <Input
              placeholder="Digite o nome da máquina"
              {...register("machineName", {
                required: "Nome da máquina é obrigatório",
              })}
            />
          </Field>

          <Field
            label="Marca"
            invalid={!!errors.mark}
            errorText={errors.mark?.message}
          >
            <Input
              placeholder="Digite o nome da marca"
              {...register("mark", {
                required: "Marca é obrigatório",
              })}
            />
          </Field>

          <Button w="15%">Cadastrar</Button>
        </Flex>
      </Flex>
    </form>
  );
};

export default CreateMachine;
